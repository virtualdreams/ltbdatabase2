using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Book: DatabaseContext
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }
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

		static public Book Default()
		{
			return new Book { Id = 0, Number = 0, Name = "", Category = Category.Default(), Created = DateTime.MinValue, Stories = new string[] { } };
		}

		/// <summary>
		/// Get all tags related to this book.
		/// </summary>
		/// <returns></returns>
		public Tag[] GetTags()
		{
			var tags = this.TagEntity.GetByBook(this.Id);

			var result = Mapper.Map<Tag[]>(tags);

			return result;
		}
	}
}