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

		public PageOffset(int pageOffset, int pageSize, int items)
		{
			if (pageSize < 0)
				PageSize = 0;
			else
				PageSize = pageSize;

			if (pageOffset < 0)
				Offset = 0;
			else
				Offset = pageOffset;

			if (Offset == 0)
				HasPrevious = false;
			else
				HasPrevious = true;

			if (Offset + PageSize < items)
				HasNext = true;
			else
				HasNext = false;
		}
	}
}