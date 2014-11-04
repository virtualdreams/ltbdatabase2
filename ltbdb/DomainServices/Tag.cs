using CS.Helper;
using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Tag: DatabaseContext
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public long References { get; set; }

		static public Tag Default()
		{
			return new Tag { Id = 0, Name = "", References = 0 };
		}

		/// <summary>
		/// Get all books related to this tag.
		/// </summary>
		/// <returns></returns>
		public Book[] GetBooks()
		{
			var books = this.BookEntity.GetByTag(this.Id);

			var result = Mapper.Map<Book[]>(books);

			return result;
		}

		#region Static methods

		/// <summary>
		/// Get all available tags.
		/// </summary>
		/// <returns>A list of tags</returns>
		static public Tag[] Get()
		{
			DatabaseContext ctx = new DatabaseContext();
			
			var tags = ctx.TagEntity.GetAll();

			return Mapper.Map<Tag[]>(tags);
		}

		/// <summary>
		/// Get a specified tag by id.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>The tag.</returns>
		static public Tag Get(int id)
		{
			DatabaseContext ctx = new DatabaseContext();
			
			var tag = ctx.TagEntity.Get(id);

			return Mapper.Map<Tag>(tag);
		}

		/// <summary>
		/// create a new tag or return existing tag.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <returns>The tag.</returns>
		static public Tag Create(string name)
		{
			DatabaseContext ctx = new DatabaseContext();

			// look if the tag exists otherwise create them
			var tag = ctx.TagEntity.GetByName(name.Filter(@"%\^#_").Escape().Trim());
			if (tag == null)
			{
				tag = ctx.TagEntity.Add(new TagDTO { Name = name.Filter(@"%\^#_").Escape().Trim() });
			}

			return Mapper.Map<Tag>(tag);
		}

		#endregion
	}
}