using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewTagModel
	{
		public BookModel[] Books { get; set; }
		public TagModel Tag { get; set; }
	}
}