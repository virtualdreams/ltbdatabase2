using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public int Category { get; set; }
		public string CategoryName { get; set; }
		public DateTime Created { get; set; }
		
		private string[] _stories = new string[] {};
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}
	}
}