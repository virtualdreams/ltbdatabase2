using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookEditContainer
	{
		public BookModel Book { get; set; }
		public CategoryModel[] Categories { get; set; }
	}
}