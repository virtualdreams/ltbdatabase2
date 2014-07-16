using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.Helper
{
	/// <summary>
	/// Class to read simple line based config key-value pairs.
	/// 
	/// Lines beginnign with '#' or ';' are treated as comments. Empty
	/// lines are ignored.
	/// Line contains a '=' are treated as a key = value - pair. Values can
	/// continued by placing a leading backslash at the of the line. Comment
	/// lines may appear in the middle of a sequence of continuation lines.
	/// 
	/// Trailing and leading whitespaces are ignored in each line, key and value.
	/// 
	/// If you want to preserve leading or trailing whitespaces of a value,
	/// sourround it with double quotes ('"').
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
	/// # The following two samples are ignored if not in continuation mode
	///  = value12
	/// some line without
	/// </summary>
	public class ConfigFile
	{
		private Hashtable _configValues = new Hashtable();

		/// <summary>
		/// Initialize new instance.
		/// </summary>
		/// <param name="filename"></param>
		public ConfigFile(string filename)
		{
			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException(filename);
			}
			Read(filename);
		}

		/// <summary>
		/// Open config file and read in.
		/// </summary>
		/// <param name="filename"></param>
		public void Read(string filename)
		{
			if (!File.Exists(filename))
				return;

			_configValues.Clear();

			string[] lines = File.ReadAllLines(filename);

			string key = "";
			string value = "";
			bool continuation = false;

			foreach (string line in lines)
			{
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
					continue;
				}

				// Test for two parts not in continuation mode
				if (pair.Length == 2 && !continuation)
				{
					key = pair[0].Trim();
					value = pair[1].Trim();

					if (key.Length == 0)
					{
						continue;
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
				
				// Add the <key> = <value> to store or override
				if (!_configValues.Contains(key))
				{
					_configValues.Add(key, value);
				}
				else
				{
					_configValues[key] = value;
				}

				continuation = false;
			}
		}

		/// <summary>
		/// Get value from config.
		/// </summary>
		/// <param name="key">The config key.</param>
		/// <returns>The value or null if not found.</returns>
		public string GetValue(string key)
		{
			return GetValue(key, null);
		}

		/// <summary>
		/// Get value from config.
		/// </summary>
		/// <param name="key">The config key.</param>
		/// <param name="defaultValue">The value or defaultValue if not found.</param>
		/// <returns></returns>
		public string GetValue(string key, string defaultValue)
		{
			if (_configValues.Contains(key))
			{
				return _configValues[key] as string;
			}
			return defaultValue;
		}
	}
}
