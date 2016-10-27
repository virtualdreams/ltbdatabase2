using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewAllContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}