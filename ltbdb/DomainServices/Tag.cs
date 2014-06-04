using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Tag: DatabaseContext
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public long References { get; set; }

		/// <summary>
		/// Get all books related to this tag.
		/// </summary>
		/// <returns></returns>
		public Book[] GetBooks()
		{
			var books = this.BookEntity.GetByTag(this.Id);

			var result = Mapper.Map<Book[]>(books);

			return result;
		}
	}
}