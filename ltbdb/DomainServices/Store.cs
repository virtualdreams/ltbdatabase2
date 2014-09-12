using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;
using AutoMapper;
using ltbdb.DomainServices.DTO;
using System.Collections;
using CS.Helper;
using ltbdb.Core;

namespace ltbdb.DomainServices
{
	public class Store: DatabaseContext
	{
		public Store()
		{

		}

		/// <summary>
		/// Get all books.
		/// </summary>
		/// <returns></returns>
		public Book[] GetBooks()
		{
			var books = this.BookEntity.GetAll();

			var result = Mapper.Map<Book[]>(books);

			return result;
		}

		/// <summary>
		/// Get a specified book.
		/// </summary>
		/// <param name="id">Id of the book</param>
		/// <returns>Book</returns>
		public Book GetBook(int id)
		{
			var book = this.BookEntity.Get(id);
			var stories = this.StoryEntity.GetByBook(id);

			var _book = Mapper.Map<Book>(book);
			var result = Mapper.Map<StoryDTO, Book>(stories, _book);

			return _book;
		}

		/// <summary>
		/// Get the 12 recently added books.
		/// </summary>
		/// <returns>List of books</returns>
		public Book[] GetRecentlyAdded()
		{
			return GetBooks().OrderByDescending(o => o.Created).Take(GlobalConfig.Get().RecentItems).ToArray();
		}

		/// <summary>
		/// Get all catgeories
		/// </summary>
		/// <returns>List of categories</returns>
		public Category[] GetCategories()
		{
			var categories = this.CategoryEntity.GetAll();
			
			var result = Mapper.Map<Category[]>(categories);

			return result;
		}

		/// <summary>
		/// Get a specified category.
		/// </summary>
		/// <param name="id">Id of the categoy</param>
		/// <returns>Category</returns>
		public Category GetCategory(int id)
		{
			var category = this.CategoryEntity.Get(id);

			var result = Mapper.Map<Category>(category);

			return result;
		}

		/// <summary>
		/// Get all tags.
		/// </summary>
		/// <returns></returns>
		public Tag[] GetTags()
		{
			var tags = this.TagEntity.GetAll();

			var result = Mapper.Map<Tag[]>(tags);

			return result;
		}

		/// <summary>
		/// Get a specified tag.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>Tag</returns>
		public Tag GetTag(int id)
		{
			var tag = this.TagEntity.Get(id);

			var result = Mapper.Map<Tag>(tag);

			return result;
		}

		/// <summary>
		/// Search for books:
		/// searchterm can be name or story
		/// </summary>
		/// <param name="term">The search term.</param>
		/// <returns>List of books match the search term.</returns>
		public Book[] Search(string term)
		{
			string eterm = term.Filter(@"%\^#_").Escape().Trim();

			if (String.IsNullOrEmpty(eterm))
			{
				return new Book[] { };
			}

			var books = this.BookEntity.GetByTerm(eterm);

			var result = Mapper.Map<Book[]>(books);

			return result;
		}

		/// <summary>
		/// Get suggestion list.
		/// searchterm can be book or story.
		/// </summary>
		/// <param name="term">The search term.</param>
		/// <returns>List of suggestions</returns>
		public string[] SuggestionList(string term)
		{
			string eterm = term.Filter(@"%\^#_").Escape().Trim();
			if (String.IsNullOrEmpty(eterm))
			{
				return new string[] { };
			}

			var suggestion = this.BookEntity.GetSuggestionList(eterm);

			return suggestion.ToArray();
		}

		/// <summary>
		/// Add a new book.
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		public Book AddBook(Book book)
		{
			//if (book == null)
			//	throw new ArgumentNullException("book");

			//if (book.Category == null)
			//	throw new Exception("Category must set to valid value.");

			//var mapper = Mapper.CreateMap<Book, BookDTO>();
			//mapper.ForMember(d => d.Category, map => map.MapFrom(s => s.Category.Id));
			//mapper.ForMember(d => d.Name, map => map.MapFrom(s => s.Name.Filter(@"%\^#").Escape().Trim()));

			//var result = Mapper.Map<BookDTO>(book);

			//try
			//{
			//	this.SqlContext.BeginTransaction();
			//	var ret = this.BookEntity.Add(result);
			//	this.SqlContext.CommitTransaction();

			//	return this.GetBook(ret.Id);
			//}
			//catch (Exception)
			//{
			//	this.SqlContext.RollbackTransaction();
			//	throw;
			//}
			throw new NotImplementedException();
		}

		public Category AddCategory(Category category)
		{
			throw new NotImplementedException();
		}

		public Tag AddTag(Tag tag)
		{
			throw new NotImplementedException();
		}

		public void DeleteBook(Book book)
		{
			throw new NotImplementedException();
		}

		public void DeleteCategory(Category category)
		{
			throw new NotImplementedException();
		}

		public void DeleteTag(Tag tag)
		{
			throw new NotImplementedException();
		}
	}
}