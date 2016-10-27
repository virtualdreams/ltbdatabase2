using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
	}
}