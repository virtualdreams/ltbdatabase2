using AutoMapper;
using ltbdb.Core.Database;
using ltbdb.Core.Database.DTO;
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

		/// <summary>
		/// Get all categories.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Category> Get()
		{
			return Mapper.Map<Category[]>(CategoryEntity.GetAll());
		}

		/// <summary>
		/// Get a category by id.
		/// </summary>
		/// <param name="id">The category id.</param>
		/// <returns></returns>
		public Category Get(int id)
		{
			var _category = CategoryEntity.Get(id);
			if (_category == null)
				return null;

			return Mapper.Map<Category>(_category);
		}

		/// <summary>
		/// Save the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public Category Save(Category category)
		{
			var _category = Mapper.Map<CategoryDTO>(category);

			if (category.Id == 0)
			{
				var _ret = CategoryEntity.Add(_category);

				return Mapper.Map<Category>(_ret);
			}
			else
			{
				var _ret = CategoryEntity.Update(_category);

				return Mapper.Map<Category>(_ret);
			}
		}

		/// <summary>
		/// Delete category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public bool Delete(Category category)
		{
			throw new NotImplementedException();
		}
	}
}