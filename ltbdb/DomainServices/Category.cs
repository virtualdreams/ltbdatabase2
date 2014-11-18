using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Get all books related to this category.
		/// </summary>
		/// <returns>A list of books.</returns>
		public Book[] GetBooks()
		{
			Database db = new Database();

			var books = db.BookEntity.GetByCategory(this.Id);

			var result = Mapper.Map<Book[]>(books);

			return result;
		}

		#region Static methods

		/// <summary>
		/// Get all available categories.
		/// </summary>
		/// <returns>A list of categories.</returns>
		static public Category[] Get()
		{
			Database db = new Database();

			var categories = db.CategoryEntity.GetAll();

			return Mapper.Map<Category[]>(categories);
		}

		/// <summary>
		/// Get a specified category by id.
		/// </summary>
		/// <param name="id">The category id.</param>
		/// <returns>The category.</returns>
		static public Category Get(int id)
		{
			Database db = new Database();

			var category = db.CategoryEntity.Get(id);

			return Mapper.Map<Category>(category);
		}

		/// <summary>
		/// Add a new category to database.
		/// </summary>
		/// <param name="model">The new category.</param>
		/// <returns>The new category</returns>
		static public Category Add(Category model)
		{
			Database db = new Database();

			var @in = Mapper.Map<CategoryDTO>(model);

			var result = db.CategoryEntity.Add(@in);

			var @out = Mapper.Map<Category>(result);

			return @out;
		}

		/// <summary>
		/// Update a existing category.
		/// </summary>
		/// <param name="model">The category.</param>
		/// <returns>The category.</returns>
		static public Category Update(Category model)
		{
			Database db = new Database();

			var @in = Mapper.Map<CategoryDTO>(model);

			var result = db.CategoryEntity.Update(@in);

			var @out = Mapper.Map<Category>(result);

			return @out;
		}

		/// <summary>
		/// Add or update a category.
		/// </summary>
		/// <param name="model">The category.</param>
		/// <returns>The category.</returns>
		static public Category Set(Category model)
		{
			var category = Category.Get(model.Id);
			model.Id = category.Id;

			if (category.Id == 0)
				return Category.Add(model);
			else
				return Category.Update(model);
		}

		#endregion
	}
}