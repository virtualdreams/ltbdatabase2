﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookModel
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public ObjectId Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// The book creation date and time.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// The image filename.
		/// </summary>
		public string Filename { get; set; }

		private string[] _stories = new string[] { };

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

		private string[] _tags = new string[] { };

		/// <summary>
		/// The tags for the book.
		/// </summary>
		public string[] Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				if (value != null)
				{
					_tags = value;
				}
			}
		}
	}

	public class BookWriteModel
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public ObjectId Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Nummer ein.")]
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib einen Titel ein.")]
		public string Title { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Kategorie ein.")]
		public string Category { get; set; }

		/// <summary>
		/// The image filename.
		/// </summary>
		public string Filename { get; set; }

		private string[] _stories = new string[] { };

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

		/// <summary>
		/// The tags for the book.
		/// </summary>
		public string Tags { get; set; }
		

		/// <summary>
		/// The posted image.
		/// </summary>
		public HttpPostedFileBase Image { get; set; }

		/// <summary>
		/// Delete image flag.
		/// </summary>
		public bool Remove { get; set; }
	}
}