﻿using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Category: DatabaseContext
	{
		public int Id { get; set; }
		public string Name { get; set; }

		static public Category Default()
		{
			return new Category { Id = 0, Name = "" };
		}

		/// <summary>
		/// Get all books related to this category.
		/// </summary>
		/// <returns>A list of books.</returns>
		public Book[] GetBooks()
		{
			var books = this.BookEntity.GetByCategory(this.Id);

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
			DatabaseContext ctx = new DatabaseContext();

			var categories = ctx.CategoryEntity.GetAll();

			return Mapper.Map<Category[]>(categories);
		}

		/// <summary>
		/// Get a specified category by id.
		/// </summary>
		/// <param name="id">The category id.</param>
		/// <returns>The category.</returns>
		static public Category Get(int id)
		{
			DatabaseContext ctx = new DatabaseContext();

			var category = ctx.CategoryEntity.Get(id);

			return Mapper.Map<Category>(category);
		}

		/// <summary>
		/// Add a new category to database.
		/// </summary>
		/// <param name="model">The new category.</param>
		/// <returns>The new category</returns>
		static public Category Add(Category model)
		{
			DatabaseContext ctx = new DatabaseContext();

			var @in = Mapper.Map<CategoryDTO>(model);

			var result = ctx.CategoryEntity.Add(@in);

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
			DatabaseContext ctx = new DatabaseContext();

			var @in = Mapper.Map<CategoryDTO>(model);

			var result = ctx.CategoryEntity.Update(@in);

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
			var r = Category.Get(model.Id);
			model.Id = r.Id;

			if (r.Id == 0)
				return Category.Add(model);
			else
				return Category.Update(model);
		}

		#endregion
	}
}