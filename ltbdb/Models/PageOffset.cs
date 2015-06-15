using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class PageOffset
	{
		public int Offset { get; private set; }
		public bool HasPrevious { get; private set; }
		public bool HasNext { get; private set; }
		public int PageSize { get; private set; }
		public int Items { get; private set; }

		public PageOffset(int pageOffset, int pageSize, int items)
		{
			Items = (items < 0) ? 0 : items;
			PageSize = (pageSize < 0) ? 0 : pageSize;
			Offset = (pageOffset < 0) ? 0 : pageOffset;
			HasPrevious = (Offset == 0) ? false : true;
			HasNext = (Offset + PageSize < Items) ? true : false;
		}
	}
}