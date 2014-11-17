using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewSearchContainer
	{
		public BookModel[] Books { get; set; }
		public string Query { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}