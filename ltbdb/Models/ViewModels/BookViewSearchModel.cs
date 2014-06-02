using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewSearchModel
	{
		public BookModel[] Books { get; set; }
		public string Query { get; set; }
	}
}