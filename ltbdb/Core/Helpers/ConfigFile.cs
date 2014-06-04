using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.Helper
{
	/// <summary>
	/// Class to read simple line based config key-value pairs
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
		/// Open config file and read it.
		/// </summary>
		/// <param name="filename"></param>
		public void Read(string filename)
		{
			if (File.Exists(filename))
			{
				_configValues.Clear();
				string[] lines = File.ReadAllLines(filename);

				foreach (string line in lines)
				{
					string c = line.Trim();
					
					if (c.StartsWith("#") || c.StartsWith(";") || c.Length == 0)
					{
						continue;
					}

					string[] pair = c.Split(new char[] {'='}, 2);
					if (pair.Length == 2)
					{
						string key = pair[0].Trim();
						string value = pair[1].Trim();

						if (!_configValues.Contains(key))
						{
							_configValues.Add(key, value);
						}
					}
				}
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
