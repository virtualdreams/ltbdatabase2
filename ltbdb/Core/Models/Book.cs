using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ltbdb.Core;
using ltbdb.Core.Helpers;
using ltbdb.Core.Database;
using ltbdb.Core.Database.DTO;

namespace ltbdb.Core.Models
{
	public class Book
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		public Category Category { get; set; }

		/// <summary>
		/// The creation date.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// The cover filename.
		/// </summary>
		public string Filename { get; set; }
		
		private string[] _stories = new string[] {};
		
		/// <summary>
		/// The stories in the book.
		/// </summary>
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}
	}
}