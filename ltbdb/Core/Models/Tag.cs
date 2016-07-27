using AutoMapper;
using ltbdb.Core.Database;
using ltbdb.Core.Database.DTO;
using ltbdb.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Models
{
	public class Tag
	{
		/// <summary>
		/// The id of the tag.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The tag name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Reference count.
		/// </summary>
		public long References { get; set; }
	}
}