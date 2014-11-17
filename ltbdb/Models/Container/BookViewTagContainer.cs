using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewTagContainer
	{
		public BookModel[] Books { get; set; }
		public TagModel Tag { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}