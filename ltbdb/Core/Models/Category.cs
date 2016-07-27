using AutoMapper;
using ltbdb.Core.Database;
using ltbdb.Core.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Models
{
	/// <summary>
	/// Represents a category.
	/// </summary>
	public class Category
	{
		/// <summary>
		/// The id of the category.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The name of the category.
		/// </summary>
		public string Name { get; set; }
	}
}