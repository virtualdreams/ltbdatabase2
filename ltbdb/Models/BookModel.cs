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
		public int Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		public CategoryModel Category { get; set; }

		/// <summary>
		/// The book creation date and time.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// The stories in the book.
		/// </summary>
		private string[] _stories = new string[] { };
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
		/// The image filename.
		/// </summary>
		public string Filename { get; set; }
	}

	public class BookWriteModel
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Nummer ein.")]
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib einen Titel ein.")]
		public string Name { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		public CategoryModel Category { get; set; }

		/// <summary>
		/// The target category.
		/// </summary>
		public int TargetCategory { get; set; }

		/// <summary>
		/// The stories in the book.
		/// </summary>
		private string[] _stories = new string[] { };
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
		/// The image filename.
		/// </summary>
		public string Filename { get; set; }

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