using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace ConfigFile
{
	/// <summary>
	/// Class to read a simple line based configuration [file] based on key and value pairs.
	/// 
	/// Lines beginning with '#' or ';' are treated as comments, empty lines are ignored.
	/// 
	/// Lines contain a '=' are treated as a key and value pair.
	/// Values can be continued by placing a trailing backslash at the end of the line.
	/// This backslash is removed from the current line.
	/// Comment lines or empty lines may appear in the middle of a sequence of continuation lines.
	/// 
	/// The key format must be in form of "[a-zA-Z_][a-zA-Z0-9_.]*".
	/// 
	/// Leading and trailing whitespaces are ignored for keys and values.
	/// 
	/// If you want to preserve leading or trailing whitespaces of a value,
	/// surround it with double quotes ('"').
	/// 
	/// If you want to ignore a trailing backslash, surround the value with double
	/// quotes.
	/// 
	/// The value can contain some escape characters:
	/// \n -> newline
	/// \t -> tabulator
	/// \\ -> escape the backslash
	/// 
	/// Unknown or empty escape characters throws an error.
	/// 
	/// A value can also defined as a here document. Such a value begins with
	/// two left angle bracket followed by a stopword ([a-zA-Z][a-zA-Z0-9]*). The parser reads
	/// the input as is until the stopword.
	/// 
	/// In this mode, comment lines and empty lines are NOT ignored. Leading and trailing
	/// whitespace is NOT removed and no escape sequence is processed.
	/// 
	/// The stopword must appear in a single line with no leading or trailing whitespace.
	/// 
	/// If you don't want start the inline mode, surround the value with double quotes.
	/// 
	/// Example:
	/// 
	/// <![CDATA[
	/// key = <<EOF
	///		my value
	/// EOF
	/// ]]>
	/// 
	/// Examples:
	/// 
	/// # Standard 
	/// key1 = value1
	/// 
	/// # Preserver whitespaces
	/// key2 = " value2 "
	/// 
	/// # Multiline value with comment
	/// key3 =	value3\
	///			value4\
	///	# One more value
	///			value5
	///		
	/// # This is ignored
	/// ;key4 = value6
	/// 
	/// # Multiline value with preserved leading and trailing whitespace.
	/// key5 =	" value7, value8,\
	///			value9, value10\
	///			value11 "
	///			
	/// # This is valid, but the value is null
	/// key6 = 
	/// 
	/// # Escape characters; 1st, write newline and tab; 2nd, write the sequence literally.
	/// key7 = value1\n\value2\tvalue3
	/// key8 = value1\\nvalue2\\tvalue3
	/// 
	/// # Escape leading backslash and ignore continuation
	/// key9 = "value\\"
	/// 
	/// # The following samples causes trouble.
	/// # Invalid key (key is null)
	///  = value12
	///  
	/// # Invalid key (contains a whitespace)
	/// key 10 = some value
	/// 
	/// # Invalid value, empty escape sequence
	/// key11 = "value\"
	/// 
	/// # Invalid value, unknown escape sequence (\v)
	/// key12 = value1\\
	/// value2
	/// </summary>
	public class ConfigReader
	{
		/// <summary>
		/// Hold the key value pairs.
		/// </summary>
		protected Hashtable _configValues = new Hashtable();
		
		/// <summary>
		/// Occurs before a new key is added.
		/// </summary>
		public event EventHandler<ConfigReaderEventArgs> OnKeyAdd;

		/// <summary>
		/// Occurs before a existing key is overwritten.
		/// </summary>
		public event EventHandler<ConfigReaderEventArgs> OnKeyChange;

		/// <summary>
		/// Hold lines for inline mode.
		/// </summary>
		private List<string> _inline = new List<string>();

		/// <summary>
		/// Initialize a new instance.
		/// </summary>
		public ConfigReader()
		{ }

		/// <summary>
		/// Initialize new instance and read the configuration from file.
		/// </summary>
		/// <param name="filename"></param>
		public ConfigReader(string filename)
		{
			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException(filename);
			}
			Open(filename);
		}

		/// <summary>
		/// Initialize new instance and read from stream.
		/// </summary>
		/// <param name="stream"></param>
		public ConfigReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentException("stream");

			Open(stream);
		}

		/// <summary>
		/// Open configuration file and clear existing values.
		/// </summary>
		/// <param name="filename"></param>
		public void Open(string filename)
		{
			if (!File.Exists(filename))
				throw new FileNotFoundException("File not found.", filename);

			_configValues.Clear();

			Parse(() => File.OpenRead(filename));
		}

		/// <summary>
		/// Read stream and clear existing values.
		/// </summary>
		/// <param name="stream"></param>
		public void Open(Stream stream)
		{
			if (stream == null)
				throw new ArgumentException("stream");

			_configValues.Clear();

			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				ms.Position = 0;

				Parse(() => ms);
			}
		}

		/// <summary>
		/// Open configuration file and add values.
		/// </summary>
		/// <param name="filename"></param>
		public void Append(string filename)
		{
			if (!File.Exists(filename))
				throw new FileNotFoundException("File not found.", filename);

			Parse(() => File.OpenRead(filename));
		}

		/// <summary>
		/// Read stream and add values.
		/// </summary>
		/// <param name="stream"></param>
		public void Append(Stream stream)
		{
			if (stream == null)
				throw new ArgumentException("stream");

			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				ms.Position = 0;

				Parse(() => ms);
			}
		}

		/// <summary>
		/// Read all lines from stream.
		/// </summary>
		/// <param name="streamProvider"></param>
		/// <returns></returns>
		private IEnumerable<string> ReadAllLines(Func<Stream> streamProvider)
		{
			using (var stream = streamProvider())
			{
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						yield return reader.ReadLine();
					}
				}
			}
		}

		/// <summary>
		/// Read a stream and parse.
		/// </summary>
		/// <param name="stream">The stream.</param>
		private void Parse(Func<Stream> stream)
		{
			var linenumber = 0;
			var key = "";
			var value = "";
			var mode = ParserMode.Normal;
			var stop = "";

			foreach (var rawline in this.ReadAllLines(stream))
			{
				linenumber++;

				var line = rawline.Trim();

				// Test for comments or empty lines and not inline mode
				if ((line.StartsWith("#") || line.StartsWith(";") || line.Length == 0) && mode != ParserMode.Here)
				{
					continue;
				}

				// normal parser mode
				if (mode == ParserMode.Normal)
				{
					// Split to <key> = <value> pair
					var pair = line.Split(new char[] { '=' }, 2);

					// Test for one part
					if (pair.Length == 1)
					{
						throw new ConfigException(String.Format("Not a key value pair. Line {0}.", linenumber));
					}

					// Test for two parts
					if (pair.Length == 2)
					{
						key = pair[0].Trim();
						value = pair[1].Trim();

						if (String.IsNullOrEmpty(key))
						{
							throw new ConfigException(String.Format("The key is null. Line {0}.", linenumber));
						}

						if (!Regex.IsMatch(key, "^[a-zA-Z_][a-zA-Z0-9_.]*$", RegexOptions.Singleline))
						{
							throw new ConfigException(String.Format("The key format for '{0}' is invalid. Line {1}.", key, linenumber));
						}
					}

					// Test for her document mode sequence
					var inline = Regex.Match(value, "^<<([a-zA-Z][a-zA-Z0-9]*)$");
					if (inline.Success)
					{
						// stopword
						stop = inline.Groups[1].Value;

						_inline.Clear();

						mode = ParserMode.Here;
						continue;
					}
				}

				// Test, if in continuation mode
				if (mode == ParserMode.Continuation)
				{
					value = String.Concat(value, line);
				}

				// Test, if in here document mode
				if (mode == ParserMode.Here)
				{
					// Test for stopword.
					if (rawline.Equals(stop))
					{
						Add(key, String.Join("\n", _inline));
						mode = ParserMode.Normal;

						_inline.Clear();

						continue;
					}

					_inline.Add(rawline);
					continue;
				}

				// Test for trailing backslash (continuation mode)
				if (value.EndsWith("\\"))
				{
					value = value.Substring(0, value.Length - 1);
					
					mode = ParserMode.Continuation;
					continue;
				}

				// Test for surrounding double quotes
				if (value.StartsWith("\"") && value.EndsWith("\""))
				{
					value = value.Substring(1, value.Length - 2);
				}

				// Unescape the value
				value = Regex.Replace(value, @"(\\(.?))", m =>
				{
					switch (m.Groups[2].Value)
					{
						case "n":
							return "\n";

						case "t":
							return "\t";

						case "\\":
							return "\\";

						case "":
							throw new ConfigException(String.Format("Empty escape sequence. Line {0}.", linenumber));

						default:
							throw new ConfigException(String.Format("Unknown escape sequence: \\{0}. Line {1}.", m.Groups[2].Value, linenumber));
					}
				});

				Add(key, value);

				mode = ParserMode.Normal;
			}

			if (mode != ParserMode.Normal)
			{
				throw new ConfigException(String.Format("End of file encountered in {0} mode.", mode.ToString().ToLower()));
			}
		}

		/// <summary>
		/// Add the key and value to store or override existing.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		private void Add(string key, string value)
		{
			var e = new ConfigReaderEventArgs(key, value);
			if (!_configValues.ContainsKey(key))
			{
				if (OnKeyAdd != null)
					OnKeyAdd(this, e);

				if (!e.Decline)
					_configValues.Add(key, value);
			}
			else
			{
				if (OnKeyChange != null)
					OnKeyChange(this, e);

				if (!e.Decline)
					_configValues[key] = value;
			}
		}

		/// <summary>
		/// Get value from configuration.
		/// </summary>
		/// <typeparam name="T">Destination type.</typeparam>
		/// <param name="key">The configuration key.</param>
		/// <param name="defaultValue">The default value if key not found.</param>
		/// <returns></returns>
		public T GetValue<T>(string key, T defaultValue) where T : IConvertible
		{
			return GetValue<T>(key, defaultValue, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Get value from configuration.
		/// </summary>
		/// <typeparam name="T">Destination type.</typeparam>
		/// <param name="key">The configuration key.</param>
		/// <param name="defaultValue">The default value if key not found.</param>
		/// <param name="provider">The format provider.</param>
		/// <returns></returns>
		public T GetValue<T>(string key, T defaultValue, IFormatProvider provider) where T : IConvertible
		{
			if (String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			if (_configValues.ContainsKey(key))
			{
				try
				{
					return (T)Convert.ChangeType(_configValues[key], typeof(T), provider);
				}
				catch (FormatException e)
				{
					throw new ConfigException(String.Format("Can't convert key '{0}' to destination type '{1}'.", key, typeof(T)), e);
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Try to get value from configuration.
		/// If the key does not exists, an exception is thrown.
		/// </summary>
		/// <typeparam name="T">Destination type.</typeparam>
		/// <param name="key">The configuration key.</param>
		/// <returns>Throws a exception if the key not found.</returns>
		public T TryGetValue<T>(string key) where T : IConvertible
		{
			return TryGetValue<T>(key, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Try to get value from configuration.
		/// If the key does not exists, an exception is thrown.
		/// </summary>
		/// <typeparam name="T">Destination type.</typeparam>
		/// <param name="key">The configuration key.</param>
		/// <param name="provider">The format provider.</param>
		/// <returns>Throws a exception if the key not found.</returns>
		public T TryGetValue<T>(string key, IFormatProvider provider) where T : IConvertible
		{
			if(String.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			if(_configValues.ContainsKey(key))
			{
				try
				{
					return (T)Convert.ChangeType(_configValues[key], typeof(T), provider);
				}
				catch (FormatException e)
				{
					throw new ConfigException(String.Format("Can't convert key '{0}' to destination type '{1}'.", key, typeof(T)), e);
				}
			}
			throw new ConfigException("The configuration key does not exists.");
		}

		/// <summary>
		/// Test if the key exists in configuration.
		/// </summary>
		/// <param name="key">The configuration key.</param>
		/// <returns></returns>
		public bool KeyExists(string key)
		{
			if (String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			return _configValues.ContainsKey(key);
		}

		/// <summary>
		/// Get all available configuration keys.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetKeys()
		{
			return _configValues.Keys.Cast<string>();
		}

		/// <summary>
		/// Get all available configuration values.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetValues()
		{
			return _configValues.Values.Cast<string>();
		}
	}

	/// <summary>
	/// Class to write simple line based configuration based on key values pairs.
	/// </summary>
	public class ConfigWriter : ConfigReader
	{
		/// <summary>
		/// Initialize new configuration writer.
		/// </summary>
		public ConfigWriter()
		{ }

		/// <summary>
		/// Initialize new configuration writer and read in an existing reader.
		/// </summary>
		/// <param name="reader">The configuration reader.</param>
		public ConfigWriter(ConfigReader reader)
			: this(reader, CultureInfo.InvariantCulture)
		{ }

		/// <summary>
		/// Initialize new configuration writer and read in an existing reader.
		/// </summary>
		/// <param name="reader">The configuration reader.</param>
		/// <param name="provider">The format provider.</param>
		public ConfigWriter(ConfigReader reader, IFormatProvider provider)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			if (provider == null)
				throw new ArgumentNullException("provider");

			foreach (var key in reader.GetKeys())
			{
				AddValue<string>(key, reader.GetValue<string>(key, ""), provider);
			}
		}

		/// <summary>
		/// Save configuration to file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public void Save(string filename)
		{
			using (var stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Writer(stream);
			}
		}

		/// <summary>
		/// Save configuration to stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public void Save(Stream stream)
		{
			Writer(stream);
		}

		/// <summary>
		/// Write key value pair to stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		private void Writer(Stream stream)
		{
			var writer = new StreamWriter(stream);

			foreach (var key in _configValues.Keys)
			{
				writer.WriteLine(String.Format("{0} = {1}", key, Prepare((string)_configValues[key])));
			}

			writer.Flush();
		}

		/// <summary>
		/// Add or set a key value pair.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void AddValue<T>(string key, T value) where T : IConvertible
		{
			AddValue<T>(key, value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Add or set a key value pair.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="provider">The format provider.</param>
		public void AddValue<T>(string key, T value, IFormatProvider provider) where T : IConvertible
		{
			if (String.IsNullOrEmpty(key))
			{
				throw new ConfigException(String.Format("The key is null or empty."));
			}

			if (!Regex.IsMatch(key, "^[a-zA-Z_][a-zA-Z0-9_.]*$", RegexOptions.Singleline))
			{
				throw new ConfigException(String.Format("The key format for '{0}' is invalid.", key));
			}

			if (!_configValues.ContainsKey(key))
			{
				_configValues.Add(key, String.Format(provider, "{0}", value));
			}
			else
			{
				_configValues[key] = String.Format(provider, "{0}", value);
			}
		}

		/// <summary>
		/// Prepare the value for writing.
		/// </summary>
		/// <param name="input">The value.</param>
		/// <returns>The prepared value.</returns>
		private string Prepare(string input)
		{
			if (Char.IsWhiteSpace(input.FirstOrDefault()) || Char.IsWhiteSpace(input.LastOrDefault()))
			{
				input = String.Format("\"{0}\"", input);
			}

			input = Regex.Replace(input, @"(\n|\t|\\)", m =>
			{
				switch (m.Groups[1].Value)
				{
					case "\n":
						return "\\n";

					case "\t":
						return "\\t";

					case "\\":
						return "\\\\";
				}

				throw new ConfigException("Unknown escape sequence.");
			});

			return input;
		}
	}

	/// <summary>
	/// ConfigFile event args.
	/// </summary>
	public class ConfigReaderEventArgs : EventArgs
	{
		/// <summary>
		/// Get the key.
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// Get the value.
		/// </summary>
		public string Value { get; private set; }

		/// <summary>
		/// Set to true to decline.
		/// </summary>
		public bool Decline { get; set; }

		/// <summary>
		/// Initialize new event arguments.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="decline">Decline value.</param>
		public ConfigReaderEventArgs(string key, string value, bool decline = false)
		{
			Key = key;
			Value = value;
			Decline = decline;
		}
	}

	/// <summary>
	/// Processing modes of the parser
	/// </summary>
	public enum ParserMode
	{
		/// <summary>
		/// Normal parsing mode.
		/// </summary>
		Normal,

		/// <summary>
		/// Continuation mode.
		/// </summary>
		Continuation,

		/// <summary>
		/// Here document mode.
		/// </summary>
		Here
	}

	/// <summary>
	/// This exception is thrown when an error in the ConfigFile occurs.
	/// </summary>
	/// <remarks>
	/// This is the base exception for all exceptions thrown in the ConfigFile
	/// </remarks>
	[Serializable]
	public class ConfigException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CS.Helper.ConfigFileException" /> class.
		/// </summary>
		public ConfigException()
			: base("ConfigFile caused an exception.")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CS.Helper.ConfigFileException" /> class.
		/// </summary>
		public ConfigException(Exception ex)
			: base("ConfigFile caused an exception.", ex)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CS.Helper.ConfigFileException" /> class.
		/// </summary>
		public ConfigException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CS.Helper.ConfigFileException" /> class.
		/// </summary>
		public ConfigException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CS.Helper.ConfigFileException" /> class.
		/// </summary>
		protected ConfigException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
