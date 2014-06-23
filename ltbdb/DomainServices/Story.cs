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
		public string Name { get; set; }

		static public Story Default()
		{
			return new Story { Id = 0, BookId = 0, Name = "" };
		}
	}
}