using AutoMapper;
using ltbdb.Core.Database;
using ltbdb.Core.Database.DTO;
using ltbdb.Core.Helpers;
using ltbdb.Core.Models;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ltbdb.Core.Services
{
	public class TagService: DatabaseContextNew
	{
		public TagService(SqlConfig config, SqlContext context)
			: base(config, context)
		{ }

		/// <summary>
		/// Get all tags.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Tag> Get()
		{
			return Mapper.Map<Tag[]>(TagEntity.GetAll());
		}

		/// <summary>
		/// Get a tag by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Tag Get(int id)
		{
			var _tag = TagEntity.Get(id);
			if (_tag == null)
				return null;

			return Mapper.Map<Tag>(_tag);
		}

		/// <summary>
		/// Get tags by book.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Tag> GetByBook(int id)
		{
			return Mapper.Map<Tag[]>(TagEntity.GetByBook(id));
		}

		/// <summary>
		/// Get tag by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Tag GetByName(string name)
		{
			var _tag = TagEntity.GetByName(name.Escape().Trim());
			if (_tag == null)
				return null;

			return Mapper.Map<Tag>(_tag);
		}

		/// <summary>
		/// Create a tag if not exists or return existing.
		/// </summary>
		/// <param name="name">The tag.</param>
		/// <returns></returns>
		public Tag Create(string name)
		{
			var _tag = TagEntity.GetByName(name.Escape().Trim());
			if (_tag == null)
			{
				_tag = TagEntity.Add(new TagDTO { Name = name });
			}

			return Mapper.Map<Tag>(_tag);
		}

		/// <summary>
		/// Update an existing tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		public Tag Update(Tag tag)
		{
			var _tag = Mapper.Map<TagDTO>(tag);

			if (tag.Id == 0)
			{
				throw new NotSupportedException();
			}
			else
			{
				var _ret = TagEntity.Update(_tag);

				return Mapper.Map<Tag>(_ret);
			}
		}

		/// <summary>
		/// Add tags to book.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <param name="tags">The tags to add.</param>
		/// <returns></returns>
		public IEnumerable<Tag> Add(int id, params string[] tags)
		{
			foreach (var tag in tags)
			{
				var _tag = Create(tag);

				var _tags = GetByBook(id);
				if (_tags.Where(s => s.Id == _tag.Id).Count() == 0)
				{
					//TODO check link?
					var link = Tag2BookEntity.Add(new Tag2BookDTO { TagId = _tag.Id, BookId = id });

					yield return new Tag { Id = _tag.Id, Name = _tag.Name, References = _tag.References };
				}
			}
		}

		/// <summary>
		/// Remove a tag from a book (unlink).
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <param name="tagid">The tag id.</param>
		/// <returns></returns>
		public bool Remove(int id, int tagid)
		{
			return Tag2BookEntity.Delete(new Tag2BookDTO { BookId = id, TagId = tagid });
		}

		/// <summary>
		/// Delete a tag.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns></returns>
		public bool Delete(Tag tag)
		{
			return TagEntity.Delete(new TagDTO { Id = tag.Id });
		}
	}
}