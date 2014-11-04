﻿using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS.Helper;
using ltbdb.Core;

namespace ltbdb.DomainServices
{
	public class Book: DatabaseContext
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }
		public DateTime Created { get; set; }
		
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

		static public Book Default()
		{
			return new Book { Id = 0, Number = 0, Name = "", Category = Category.Default(), Created = DateTime.MinValue, Stories = new string[] { } };
		}

		/// <summary>
		/// Get all tags related to this book.
		/// </summary>
		/// <returns></returns>
		public Tag[] GetTags()
		{
			//if (this.Id == 0)
			//	throw new Exception("Invalid book id.");

			var tags = this.TagEntity.GetByBook(this.Id);

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
			//if(this.Id == 0)
			//	throw new Exception("Invalid book id.");

			// create the tag
			var tag = Tag.Create(name);

			// test if a link already exists
			var tags = GetTags();
			if (tags.Where(s => s.Id == tag.Id).Count() == 0)
			{
				// link the tag to the book
				var link = this.Tag2BookEntity.Add(new Tag2BookDTO { BookId = this.Id, TagId = tag.Id });
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
		/// Unlink a tag from book.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>True on success.</returns>
		public bool Unlink(int id)
		{
			var _tag = Tag.Get(id);
			return this.Tag2BookEntity.Delete(new Tag2BookDTO { TagId = _tag.Id, BookId = this.Id });
		}

		#region Static methods

		/// <summary>
		/// Get all available books from database.
		/// </summary>
		/// <returns>A list of books.</returns>
		static public Book[] Get()
		{
			DatabaseContext ctx = new DatabaseContext();
			
			var books = ctx.BookEntity.GetAll();

			return Mapper.Map<Book[]>(books);
		}

		/// <summary>
		/// Get a specified book by id.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns>The book.</returns>
		static public Book Get(int id)
		{
			DatabaseContext ctx = new DatabaseContext();
			
			var book = ctx.BookEntity.Get(id);

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
			DatabaseContext ctx = new DatabaseContext();

			var @in = Mapper.Map<BookDTO>(model);

			var result = ctx.BookEntity.Add(@in);

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
			DatabaseContext ctx = new DatabaseContext();

			var @in = Mapper.Map<BookDTO>(model);

			var result = ctx.BookEntity.Update(@in);

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
			var r = Book.Get(model.Id);
			model.Id = r.Id;

			if (r.Id == 0)
				return Book.Add(model);
			else
				return Book.Update(model);
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
			DatabaseContext ctx = new DatabaseContext();
			
			string eterm = term.Filter(@"%\^#_").Escape().Trim();

			if (String.IsNullOrEmpty(eterm))
			{
				return new Book[] { };
			}

			var books = ctx.BookEntity.GetByTerm(eterm);

			return Mapper.Map<Book[]>(books);
		}

		/// <summary>
		/// Get a suggestion list. The term can be book or story.
		/// </summary>
		/// <param name="term">The search term.</param>
		/// <returns>A list of suggestions.</returns>
		static public string[] SuggestionList(string term)
		{
			DatabaseContext ctx = new DatabaseContext();
			
			string eterm = term.Filter(@"%\^#_").Escape().Trim();
			if (String.IsNullOrEmpty(eterm))
			{
				return new string[] { };
			}

			var suggestion = ctx.BookEntity.GetSuggestionList(eterm);

			return suggestion.ToArray();
		}

		#endregion
	}
}