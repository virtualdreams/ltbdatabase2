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
	/// Class to read simple line based configuration based on key value pairs.
	/// 
	/// Lines beginning with '#' or ';' are treated as comments.
	/// Empty lines are ignored.
	/// Line contains a '=' are treated as a key = value - pair. Values can
	/// continued by placing a leading backslash at the of the line. Comment
	/// lines may appear in the middle of a sequence of continuation lines.
	/// 
	/// The key format must be "^[a-zA-Z_][a-zA-Z0-9_.]*$".
	/// 
	/// Trailing and leading whitespaces are ignored in each line, key and value.
	/// 
	/// If you want to preserve leading or trailing whitespaces of a value,
	/// sourround it with double quotes ('"').
	/// 
	/// The value can contain a '\n' for a newline. You can escape this with '\\n'.
	/// 
	/// Examples:
	/// 
	/// # Standard 
	/// key1 = value1
	/// key2 = " value2 "
	/// 
	/// # Multiline value
	/// key3 =	value3\
	///			value4\
	///	# One more value
	///			value5
	///		
	/// # This is ignored
	/// ;key4 = value6
	/// 
	/// # The leading and trailing whitespace are preseverd
	/// key5 =	" value7, value8,\
	///			value9, value10\
	///			value11 "
	///			
	/// # This is valid, but the value is null
	/// key6 = 
	/// 
	/// # The following two samples causes an exception.
	///  = value12
	/// key 7 = some value
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
		/// Initialize a new instance.
		/// </summary>
		public ConfigReader()
		{ }

		/// <summary>
		/// Initialize new instance and read the configuration file.
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
		/// Initialize new instance and read the stream.
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
			var continuation = false;

			foreach (var cline in this.ReadAllLines(stream))
			{
				linenumber++;

				var line = cline.Trim();

				// Test for comments or empty lines
				if (line.StartsWith("#") || line.StartsWith(";") || line.Length == 0)
				{
					continue;
				}

				// Split to <key> = <value> pair
				string[] pair = line.Split(new char[] { '=' }, 2);

				// Test for one part and not in continuation mode
				if (pair.Length == 1 && !continuation)
				{
					throw new ConfigException(String.Format("No key value pair found on line {0}.", linenumber));
				}

				// Test for two parts not in continuation mode
				if (pair.Length == 2 && !continuation)
				{
					key = pair[0].Trim();
					value = pair[1].Trim();

					if (String.IsNullOrEmpty(key))
					{
						throw new ConfigException(String.Format("The key is null on line {0}.", linenumber));
					}

					if (!Regex.IsMatch(key, "^[a-zA-Z_][a-zA-Z0-9_.]*$", RegexOptions.Singleline))
					{
						throw new ConfigException(String.Format("The key format for '{0}' is invalid on line {1}.", key, linenumber));
					}
				}

				// Test for continuation mode
				if (continuation)
				{
					value = String.Concat(value, line);
				}

				// Test for trailing backslash
				if (value.EndsWith("\\"))
				{
					value = value.TrimEnd('\\'); //.Substring(0, value.Length - 1);
					continuation = true;
					continue;
				}

				// Test for surrounding double quotes
				if (value.StartsWith("\"") && value.EndsWith("\""))
				{
					value = value.Trim('\"'); //.Substring(1, value.Length - 2);
				}
				
				// escape newline and replace escaped newline
				value = Regex.Replace(value, @"((?<!\\)\\n)", Environment.NewLine);
				value = Regex.Replace(value, @"(\\\\)(n)", "\\$2");

				// Add the <key> = <value> to store or override
				var e = new ConfigReaderEventArgs(key, value);
				if (!_configValues.ContainsKey(key))
				{
					if (OnKeyAdd != null)
						OnKeyAdd(this, e);

					if(!e.Decline)
						_configValues.Add(key, value);
				}
				else
				{
					if (OnKeyChange != null)
						OnKeyChange(this, e);

					if(!e.Decline)
						_configValues[key] = value;
				}

				continuation = false;
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
		/// If the key does not exists, a exception is thrown.
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
		/// If the key does not exists, a exception is thrown.
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
		//private Hashtable _configValues = new Hashtable();

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
		/// Prepare value.
		/// </summary>
		/// <param name="input">The value.</param>
		/// <returns>The prepared value.</returns>
		private string Prepare(string input)
		{
			input = input.Replace("\n", "\\n");

			if (Char.IsWhiteSpace(input.FirstOrDefault()) || Char.IsWhiteSpace(input.LastOrDefault()))
			{
				input = String.Format("\"{0}\"", input);
			}

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
