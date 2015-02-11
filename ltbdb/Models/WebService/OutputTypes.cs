using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models.WebService
{
	public class Book
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Number { get; set; }
		public Category Category { get; set; }
		public DateTime Created { get; set; }
		public string[] Stories { get; set; }
		public Tag[] Tags { get; set; }
	}

	public class Tag
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public long References { get; set; }
	}

	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}