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

		/// <summary>
		/// Get all books related ti this category.
		/// </summary>
		/// <returns></returns>
		public Book[] GetBooks()
		{
			var books = this.BookEntity.GetByCategory(this.Id);

			var result = Mapper.Map<Book[]>(books);

			return result;
		}
	}
}