using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

namespace ltbdb.Core.Database.DTO
{
	public class BookDTO
	{
		[Required]
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		private string _stories = String.Empty;
		public string Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
					_stories = value;
			}
		}
		public int Category { get; set; }
		public string CategoryName { get; set; }
		public DateTime Added { get; set; }

		[Alias("image")]
		public string Filename { get; set; }
	}
}