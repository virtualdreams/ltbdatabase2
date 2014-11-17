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
	public class Tag
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
			Database db = new Database();

			var books = db.BookEntity.GetByTag(this.Id);

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
			Database db = new Database();
			
			var tags = db.TagEntity.GetAll();

			return Mapper.Map<Tag[]>(tags);
		}

		/// <summary>
		/// Get a specified tag by id.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>The tag.</returns>
		static public Tag Get(int id)
		{
			Database db = new Database();
			
			var tag = db.TagEntity.Get(id);

			return Mapper.Map<Tag>(tag);
		}

		/// <summary>
		/// create a new tag or return existing tag.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <returns>The tag.</returns>
		[Obsolete("Replace or check with Add", false)]
		static public Tag Create(string name)
		{
			Database db = new Database();

			// look if the tag exists otherwise create them
			var tag = db.TagEntity.GetByName(name.Filter(@"%\^#_").Escape().Trim());
			if (tag == null)
			{
				tag = db.TagEntity.Add(new TagDTO { Name = name.Filter(@"%\^#_").Escape().Trim() });
			}

			return Mapper.Map<Tag>(tag);
		}

		/// <summary>
		/// Add a new tag to database.
		/// </summary>
		/// <param name="model">The new tag.</param>
		/// <returns>The new tag.</returns>
		static public Tag Add(Tag model)
		{
			Database db = new Database();

			var @in = Mapper.Map<TagDTO>(model);

			var result = db.TagEntity.Add(@in);

			var @out = Mapper.Map<Tag>(result);

			return @out;
		}

		/// <summary>
		/// Update a tag.
		/// </summary>
		/// <param name="model">The tag.</param>
		/// <returns>The tag.</returns>
		static public Tag Update(Tag model)
		{
			Database db = new Database();

			var @in = Mapper.Map<TagDTO>(model);

			var result = db.TagEntity.Update(@in);

			var @out = Mapper.Map<Tag>(result);

			return @out;
		}

		/// <summary>
		/// Delete a tag.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>True on success.</returns>
		static public bool Delete(int id)
		{
			Database db = new Database();

			var tag = Get(id);

			return db.TagEntity.Delete(Mapper.Map<TagDTO>(tag));
		}

		/// <summary>
		/// Add o update a tag.
		/// </summary>
		/// <param name="model">The tag.</param>
		/// <returns>The tag.</returns>
		static public Tag Set(Tag model)
		{
			var r = Tag.Get(model.Id);
			model.Id = r.Id;

			if (r.Id == 0)
				return Tag.Add(model);
			else
				return Tag.Update(model);
		}

		#endregion
	}
}