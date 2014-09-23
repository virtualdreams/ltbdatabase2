using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS.Helper;

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
			if (this.Id == 0)
				throw new Exception("Book id not set.");

			var tags = this.TagEntity.GetByBook(this.Id);

			var result = Mapper.Map<Tag[]>(tags);

			return result;
		}

		/// <summary>
		/// Add a tag to book.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public Tag AddTag(string name)
		{
			if(this.Id == 0)
				throw new Exception("Book id not set.");
			
			// look if the tag exists otherwise create them
			var tag = this.TagEntity.GetByName(name.Filter(@"%\^#_").Escape().Trim());
			if (tag == null)
			{
				tag = this.TagEntity.Add(new TagDTO { Name = name.Filter(@"%\^#_").Escape().Trim() });
			}

			// test if a link already exists
			var tags = GetTags();
			var c = tags.Where(s => s.Id == tag.Id);
			if (c.Count() == 0)
			{
				// link the tag to the book
				var link = this.Tag2BookEntity.Add(new Tag2BookDTO { BookId = this.Id, TagId = tag.Id });

				return Mapper.Map<Tag>(tag);
			}

			return null;
		}

		/// <summary>
		/// Add a list of tags and link them to the book. If the link already exists, the return set
		/// doesn't contain the new tag.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Tag[] AddTags(params string[] name)
		{
			List<Tag> tags = new List<Tag>();
			foreach (string n in name)
			{
				Tag tag = this.AddTag(n);
				if (tag != null)
				{
					tags.Add(tag);
				}
			}
			return tags.ToArray();
		}
	}
}