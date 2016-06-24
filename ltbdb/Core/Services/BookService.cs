using AutoMapper;
using ltbdb.Core.Helpers;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ltbdb.Core.Database;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class BookService: DatabaseContextNew
	{
		public BookService(SqlConfig config, SqlContext context)
			: base(config, context)
		{ }

		/// <summary>
		/// Get all books.
		/// </summary>
		/// <returns>List of books.</returns>
		public IEnumerable<Book> Get()
		{
			return Mapper.Map<Book[]>(BookEntity.GetAll());
		}

		/// <summary>
		/// Get book by id.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns></returns>
		public Book Get(int id)
		{
			var _book = BookEntity.Get(id);
			if (_book == null)
				return null;

			return Mapper.Map<Book>(_book);
		}

		/// <summary>
		/// Get books by category.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Book> GetByCategory(int id)
		{
			return Mapper.Map<Book[]>(BookEntity.GetByCategory(id));
		}

		/// <summary>
		/// Get books by tag.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Book> GetByTag(int id)
		{
			return Mapper.Map<Book[]>(BookEntity.GetByTag(id));
		}

		/// <summary>
		/// Get the recently added books.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Book> RecentlyAdded()
		{
			return Get().OrderByDescending(o => o.Created).Take(GlobalConfig.Get().RecentItems);
		}

		/// <summary>
		/// Search for book by term.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public IEnumerable<Book> Search(string term)
		{
			var _escapedTerm = term.Escape().EscapeForSearch().Trim();

			if (String.IsNullOrEmpty(_escapedTerm))
			{
				return Enumerable.Empty<Book>();
			}

			return Mapper.Map<Book[]>(BookEntity.GetByTerm(_escapedTerm));
		}
	}
}