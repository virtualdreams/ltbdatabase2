﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookViewDetailModel
	{
		public BookModel Book { get; set; }
		public TagModel[] Tags { get; set; }
		public StoryModel[] Stories { get; set; }
	}
}