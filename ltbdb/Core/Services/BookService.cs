using AutoMapper;
using ltbdb.Core.Helpers;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ltbdb.Core.Database;
using ltbdb.Core.Models;
using ltbdb.Core.Database.DTO;
using System.IO;

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

		/// <summary>
		/// Get a suggestion list.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public IEnumerable<string> Suggestion(string term)
		{
			var _escapedTerm = term.Escape().EscapeForSearch().Trim();
			if (String.IsNullOrEmpty(_escapedTerm))
			{
				return Enumerable.Empty<string>();
			}

			return BookEntity.GetSuggestionList(_escapedTerm);
		}

		/// <summary>
		/// Save the book.
		/// </summary>
		/// <param name="book">The new or updated book.</param>
		public Book Save(Book book)
		{
			var _book = Mapper.Map<BookDTO>(book);
			
			if (book.Id == 0)
			{
				var _ret = BookEntity.Add(_book);

				return Mapper.Map<Book>(_ret);
			}
			else
			{
				var _ret = BookEntity.Update(_book);

				return Mapper.Map<Book>(_ret);
			}
		}

		/// <summary>
		/// Delete a book.
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		public bool Delete(Book book)
		{
			if (ImageStore.Exists(book.Filename))
			{
				ImageStore.Remove(book.Filename, true);
			}
			
			return BookEntity.Delete(new BookDTO { Id = book.Id });
		}

		/// <summary>
		/// Set the book image. If stream is set to null, is the image removed.
		/// </summary>
		/// <param name="id">The id of the book.</param>
		/// <param name="stream">The stream that hold the uploaded image.</param>
		public bool SetImage(Book book, Stream stream)
		{
			if (stream == null)
			{
				//Remove image
				ImageStore.Remove(book.Filename, true);
				book.Filename = null;
			}
			else
			{
				//Save image
				var filename = ImageStore.Save(stream, true);
				if (String.IsNullOrEmpty(filename))
				{
					return false;
				}
				book.Filename = filename;
			}

			var _update = Mapper.Map<BookDTO>(book);

			BookEntity.UpdateImage(_update);

			return true;
		}
	}
}