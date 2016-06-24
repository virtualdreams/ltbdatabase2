using AutoMapper;
using ltbdb.Core.Database;
using ltbdb.Core.Models;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Services
{
	public class CategoryService: DatabaseContextNew
	{
		public CategoryService(SqlConfig config, SqlContext context)
			: base(config, context)
		{ }

		public IEnumerable<Category> Get()
		{
			return Mapper.Map<Category[]>(CategoryEntity.GetAll());
		}

		public Category Get(int id)
		{
			var _category = CategoryEntity.Get(id);
			if (_category == null)
				return null;

			return Mapper.Map<Category>(_category);
		}
	}
}