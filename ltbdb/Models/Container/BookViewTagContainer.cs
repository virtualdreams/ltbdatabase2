using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewTagContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public string Tag { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}