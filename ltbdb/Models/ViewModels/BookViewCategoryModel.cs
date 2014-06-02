using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewCategoryModel
	{
		public BookModel[] Books { get; set; }
		public CategoryModel Category { get; set; }
	}
}