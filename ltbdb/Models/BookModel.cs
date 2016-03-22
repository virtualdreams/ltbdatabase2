using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class BookModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Bitte gib eine Nummer ein.")]
		public int? Number { get; set; }
		
		[Required(ErrorMessage="Bitte gib einen Titel ein.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Bitte wähle eine Kategorie aus.")]
		public int Category { get; set; }
		
		public string CategoryName { get; set; }
		
		public DateTime Created { get; set; }
		
		private string[] _stories = new string[] {};
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

		public string Filename { get; set; }

		public HttpPostedFileBase Image { get; set; }
		public bool Remove { get; set; }
	}
}