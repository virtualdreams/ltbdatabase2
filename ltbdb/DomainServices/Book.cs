using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ltbdb.Core;
using ltbdb.Core.Helpers;

namespace ltbdb.DomainServices
{
	public class Book
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }
		public DateTime Created { get; set; }
		public string Filename { get; set; }
		
		private string[] _stories = new string[] {};
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}

		/// <summary>
		/// Get all tags related to this book.
		/// </summary>
		/// <returns></returns>
		public Tag[] GetTags()
		{
			Database db = new Database();

			var tags = db.TagEntity.GetByBook(this.Id);

			var result = Mapper.Map<Tag[]>(tags);

			return result;
		}

		/// <summary>
		/// Link a tag to the book. Return null if the link already exists.
		/// </summary>
		/// <param name="tag">The tag name.</param>
		/// <returns>The tag.</returns>
		public Tag AddTag(string name)
		{
			Database db = new Database();
			
			// TODO validate the book before link tags

			// create the tag
			var tag = Tag.Create(name);

			// test if a link already exists
			var tags = GetTags();
			if (tags.Where(s => s.Id == tag.Id).Count() == 0)
			{
				// link the tag to the book
				var link = db.Tag2BookEntity.Add(new Tag2BookDTO { BookId = this.Id, TagId = tag.Id });
				return Mapper.Map<Tag>(tag);
			}

			return null;
		}

		/// <summary>
		/// Add a list of tags and link them to the book. If the link already exists, the return set
		/// doesn't contain the added tag.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Tag[] AddTags(params string[] name)
		{
			List<Tag> tags = new List<Tag>();
			foreach (string n in name)
			{
				Tag tag = this.AddTag(n);
				if (tag != null)
				{
					tags.Add(tag);
				}
			}
			return tags.ToArray();
		}

		/// <summary>
		/// Unlink a tag this book.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>True on success.</returns>
		public bool Unlink(int id)
		{
			Database db = new Database();

			var _tag = Tag.Get(id);
			
			return db.Tag2BookEntity.Delete(new Tag2BookDTO { TagId = _tag.Id, BookId = this.Id });
		}

		/// <summary>
		/// Set image to book.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public Book SetImage(string filename)
		{
			Database db = new Database();

			var book = Get(this.Id);

			ImageStore.Remove(book.Filename);

			book.Filename = filename;

			var _book = Mapper.Map<BookDTO>(book);

			return Mapper.Map<Book>(db.BookEntity.UpdateImage(_book));
		}

		#region Static methods

		/// <summary>
		/// Get all available books from database.
		/// </summary>
		/// <returns>A list of books.</returns>
		static public Book[] Get()
		{
			Database db = new Database();
			
			var books = db.BookEntity.GetAll();

			return Mapper.Map<Book[]>(books);
		}

		/// <summary>
		/// Get a specified book by id.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns>The book.</returns>
		static public Book Get(int id)
		{
			Database db = new Database();
			
			var book = db.BookEntity.Get(id);

			var _book = Mapper.Map<Book>(book);

			return _book;
		}

		/// <summary>
		/// Add a new book to database.
		/// </summary>
		/// <param name="model">The new book.</param>
		/// <returns>The new book.</returns>
		static public Book Add(Book model)
		{
			Database db = new Database();

			var @in = Mapper.Map<BookDTO>(model);

			var result = db.BookEntity.Add(@in);

			var @out = Mapper.Map<Book>(result);
			
			return @out;
		}

		/// <summary>
		/// Update a existing book.
		/// </summary>
		/// <param name="model">The book.</param>
		/// <returns>The book.</returns>
		static public Book Update(Book model)
		{
			Database db = new Database();

			var @in = Mapper.Map<BookDTO>(model);

			var result = db.BookEntity.Update(@in);

			var @out = Mapper.Map<Book>(result);

			return @out;
		}

		/// <summary>
		/// Add or update a book.
		/// </summary>
		/// <param name="model">The book.</param>
		/// <returns>The book.</returns>
		static public Book Set(Book model)
		{
			var book = Get(model.Id);
			model.Id = book.Id;

			if (book.Id == 0)
				return Book.Add(model);
			else
				return Book.Update(model);
		}

		static public bool Delete(int id)
		{
			Database db = new Database();

			var book = Get(id);

			if(ImageStore.Exists(book.Filename))
			{
				ImageStore.Remove(book.Filename);
			}

			return db.BookEntity.Delete(new BookDTO { Id = id });
		}

		/// <summary>
		/// Get the recently added books.
		/// </summary>
		/// <returns>A list of books.</returns>
		static public Book[] GetRecentlyAdded()
		{
			return Book.Get().OrderByDescending(o => o.Created).Take(GlobalConfig.Get().RecentItems).ToArray();
		}

		/// <summary>
		/// Search for books or stories.
		/// </summary>
		/// <param name="term">The search term.</param>
		/// <returns>A list of books.</returns>
		static public Book[] Search(string term)
		{
			Database db = new Database();
			
			//TODO modify filter and escape

			string eterm = term.Filter(@"%\^#_").Escape().Trim();

			if (String.IsNullOrEmpty(eterm))
			{
				return new Book[] { };
			}

			var books = db.BookEntity.GetByTerm(eterm);

			return Mapper.Map<Book[]>(books);
		}

		/// <summary>
		/// Get a suggestion list. The term can be book or story.
		/// </summary>
		/// <param name="term">The search term.</param>
		/// <returns>A list of suggestions.</returns>
		static public string[] SuggestionList(string term)
		{
			Database db = new Database();
			
			string eterm = term.Filter(@"%\^#_").Escape().Trim();
			if (String.IsNullOrEmpty(eterm))
			{
				return new string[] { };
			}

			var suggestion = db.BookEntity.GetSuggestionList(eterm);

			return suggestion.ToArray();
		}

		#endregion
	}
}