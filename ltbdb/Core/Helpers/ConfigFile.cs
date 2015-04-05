using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace ConfigFile
{
	/// <summary>
	/// Class to read simple line based configuration based on keys and values.
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
		private Hashtable _configValues = new Hashtable();
		
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
		/// Initialize new instance and read the config file.
		/// </summary>
		/// <param name="filename"></param>
		public ConfigReader(string filename)
		{
			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException(filename);
			}
			Read(filename);
		}

		/// <summary>
		/// Open configuration file and clear existing values.
		/// </summary>
		/// <param name="filename"></param>
		public void Read(string filename)
		{
			if (!File.Exists(filename))
				return;

			Read(File.OpenRead(filename));
		}

		/// <summary>
		/// Read stream and clear existing values.
		/// </summary>
		/// <param name="stream"></param>
		public void Read(Stream stream)
		{
			_configValues.Clear();

			Parse(() => stream);
		}

		/// <summary>
		/// Open configuration file and add values.
		/// </summary>
		/// <param name="filename"></param>
		public void Add(string filename)
		{
			if (!File.Exists(filename))
				return;

			Add(File.OpenRead(filename));
		}

		/// <summary>
		/// Read stream and add values.
		/// </summary>
		/// <param name="stream"></param>
		public void Add(Stream stream)
		{
			Parse(() => stream);
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
		/// <param name="filename"></param>
		private void Parse(Func<Stream> stream)
		{
			var lines = this.ReadAllLines(stream);

			int fline = 0;
			string key = "";
			string value = "";
			bool continuation = false;

			foreach (string line in lines)
			{
				fline++;

				string c = line.Trim();

				// Test for comments or empty lines
				if (c.StartsWith("#") || c.StartsWith(";") || c.Length == 0)
				{
					continue;
				}

				// Split to <key> = <value> pair
				string[] pair = c.Split(new char[] { '=' }, 2);

				// Test for one part and not in continuation mode
				if (pair.Length == 1 && !continuation)
				{
					throw new ConfigException(String.Format("No key value pair found on line {0}.", fline));
				}

				// Test for two parts not in continuation mode
				if (pair.Length == 2 && !continuation)
				{
					key = pair[0].Trim();
					value = pair[1].Trim();

					if (String.IsNullOrEmpty(key))
					{
						throw new ConfigException(String.Format("The key is null on line {0}.", fline));
					}

					if (!Regex.IsMatch(key, "^[a-zA-Z_][a-zA-Z0-9_.]*$", RegexOptions.Singleline))
					{
						throw new ConfigException(String.Format("The key format for '{0}' is invalid on line {1}.", key, fline));
					}
				}

				// Test for continuation mode
				if (continuation)
				{
					value = String.Concat(value, c);
				}

				// Test for trailing backslash
				if (value.EndsWith("\\"))
				{
					value = value.Substring(0, value.Length - 1);
					continuation = true;
					continue;
				}

				// Test for surrounding double quotes
				if (value.StartsWith("\"") && value.EndsWith("\""))
				{
					value = value.Substring(1, value.Length - 2);
				}
				
				// escape newline and replace escaped newline
				value = Regex.Replace(value, @"((?<!\\)\\n)", Environment.NewLine);
				value = Regex.Replace(value, @"(\\\\)(n)", "\\$2");

				// Add the <key> = <value> to store or override
				var e = new ConfigReaderEventArgs(key, value);
				if (!_configValues.Contains(key))
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
		/// <param name="key">The config key.</param>
		/// <param name="defaultValue">The default value if key not found.</param>
		/// <returns></returns>
		public string GetValue(string key, string defaultValue)
		{
			if (String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");
			
			if (_configValues.ContainsKey(key))
			{
				return _configValues[key] as string;
			}
			return defaultValue;
		}

		/// <summary>
		/// Get value from configuration.
		/// </summary>
		/// <typeparam name="T">Destination type.</typeparam>
		/// <param name="key">The config key.</param>
		/// <param name="defaultValue">The default value if key not found.</param>
		/// <returns></returns>
		public T GetValue<T>(string key, T defaultValue) where T : IConvertible
		{
			if (String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			if (_configValues.ContainsKey(key))
			{
				try
				{
					return (T)Convert.ChangeType(_configValues[key], typeof(T));
				}
				catch (FormatException e)
				{
					throw new ConfigException(String.Format("Can't convert key '{0}' to destination type '{1}'.", key, typeof(T)), e);
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Test if key exists in config.
		/// </summary>
		/// <param name="key">The config key.</param>
		/// <returns></returns>
		public bool KeyExists(string key)
		{
			if (String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			return _configValues.ContainsKey(key);
		}

		/// <summary>
		/// Get all available config keys.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetKeys()
		{
			return _configValues.Keys.Cast<string>().ToArray();
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
