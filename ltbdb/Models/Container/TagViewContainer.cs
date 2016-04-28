using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class TagViewContainer
	{
		public TagModel[] Tags { get; set; }
		public TagModel[] UnreferencedTags { get; set; }
	}
}