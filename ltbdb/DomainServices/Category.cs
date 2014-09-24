using AutoMapper;
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
		static public Category[] GetCategories()
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
		static public Category GetCategory(int id)
		{
			DatabaseContext ctx = new DatabaseContext();

			var category = ctx.CategoryEntity.Get(id);

			return Mapper.Map<Category>(category);
		}

		#endregion
	}
}