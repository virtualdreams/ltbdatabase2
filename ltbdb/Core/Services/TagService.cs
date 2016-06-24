using AutoMapper;
using ltbdb.Core.Database;
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
		/// Get a single tag by id.
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
	}
}