using AutoMapper;
using ltbdb.Core.Database;
using ltbdb.Core.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Models
{
	/// <summary>
	/// Represents a category.
	/// </summary>
	public class Category
	{
		/// <summary>
		/// The id of the category.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The name of the category.
		/// </summary>
		public string Name { get; set; }

		///// <summary>
		///// Get all books related to this category.
		///// </summary>
		///// <returns>A list of books.</returns>
		//public Book[] GetBooks()
		//{
		//	DatabaseContext db = new DatabaseContext();

		//	var books = db.BookEntity.GetByCategory(this.Id);

		//	var result = Mapper.Map<Book[]>(books);

		//	return result;
		//}

		///// <summary>
		///// Move all contents from this category to another one.
		///// </summary>
		///// <param name="to">The new category.</param>
		///// <returns>This instance.</returns>
		//public bool MoveTo(Category to)
		//{
		//	var _db = new DatabaseContext();

		//	var _from = Mapper.Map<CategoryDTO>(this);
		//	var _to = Mapper.Map<CategoryDTO>(to);

		//	return _db.CategoryEntity.Move(_from, _to);
		//}

		//#region Static methods

		///// <summary>
		///// Get all available categories.
		///// </summary>
		///// <returns>A list of categories.</returns>
		//static public Category[] Get()
		//{
		//	DatabaseContext db = new DatabaseContext();

		//	var categories = db.CategoryEntity.GetAll();

		//	return Mapper.Map<Category[]>(categories);
		//}

		///// <summary>
		///// Get a specified category by id.
		///// </summary>
		///// <param name="id">The category id.</param>
		///// <returns>The category.</returns>
		//static public Category Get(int id)
		//{
		//	DatabaseContext db = new DatabaseContext();

		//	var category = db.CategoryEntity.Get(id);

		//	return Mapper.Map<Category>(category);
		//}

		///// <summary>
		///// Add a new category to database.
		///// </summary>
		///// <param name="model">The new category.</param>
		///// <returns>The new category</returns>
		//static public Category Add(Category model)
		//{
		//	DatabaseContext db = new DatabaseContext();

		//	var @in = Mapper.Map<CategoryDTO>(model);

		//	var result = db.CategoryEntity.Add(@in);

		//	var @out = Mapper.Map<Category>(result);

		//	return @out;
		//}

		///// <summary>
		///// Update a existing category.
		///// </summary>
		///// <param name="model">The category.</param>
		///// <returns>The category.</returns>
		//static public Category Update(Category model)
		//{
		//	DatabaseContext db = new DatabaseContext();

		//	var @in = Mapper.Map<CategoryDTO>(model);

		//	var result = db.CategoryEntity.Update(@in);

		//	var @out = Mapper.Map<Category>(result);

		//	return @out;
		//}

		///// <summary>
		///// Add or update a category.
		///// </summary>
		///// <param name="model">The category.</param>
		///// <returns>The category.</returns>
		//static public Category Set(Category model)
		//{
		//	var category = Category.Get(model.Id);
		//	if (category != null)
		//		model.Id = category.Id;

		//	if (model.Id == 0)
		//		return Category.Add(model);
		//	else
		//		return Category.Update(model);
		//}

		///// <summary>
		///// Delete a category and all related contents.
		///// </summary>
		///// <param name="id">The id of the category.</param>
		///// <returns></returns>
		//static public bool Delete(int id)
		//{
		//	var _db = new DatabaseContext();

		//	return _db.CategoryEntity.Delete(new CategoryDTO { Id = id });
		//}

		//#endregion
	}
}