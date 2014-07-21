using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Story
	{
		public int Id { get; set; }
		public int BookId { get; set; }
		
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

		static public Story Default()
		{
			return new Story { Id = 0, BookId = 0, Stories = new string[] { } };
		}
	}
}