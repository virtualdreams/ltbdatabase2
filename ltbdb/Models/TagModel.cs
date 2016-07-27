using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	/// <summary>
	/// Model to edit or view a tag.
	/// </summary>
	public class TagModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage="Bitte gib einen Namen ein.")]
		public string Name { get; set; }
		public long References { get; set; }
	}

	/// <summary>
	/// Model to add a tag to book.
	/// </summary>
	public class AddTagModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Bitte gib einen Namen ein.")]
		public string Tags { get; set; }
	}

	/// <summary>
	/// Model to unlink a tag from book.
	/// </summary>
	public class RemoveTagModel
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The tag id.
		/// </summary>
		public int TagId { get; set; }
	}
}