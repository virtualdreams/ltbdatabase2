using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;
using AutoMapper;
using ltbdb.DomainServices.DTO;
using System.Collections;

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
			BookRepository bookRepo = new BookRepository(this.Config, this.Context);

			var bookDTO = bookRepo.GetAll();

			var mapper = Mapper.CreateMap<BookDTO, Book>();
			mapper.ForMember(d => d.Created, map => map.MapFrom(s => s.Added));
			mapper.ForMember(d => d.Category, map => map.MapFrom(s => new Category { Id = s.Category, Name = s.CategoryName }));

			var books = Mapper.Map<IEnumerable<BookDTO>, Book[]>(bookDTO);

			return books;
		}

		/// <summary>
		/// Get the 12 recently added books.
		/// </summary>
		/// <returns>List of books</returns>
		public Book[] GetRecentlyAdded()
		{
			return GetBooks().OrderByDescending(o => o.Created).Take(12).ToArray();
		}

		public Category[] GetCategories()
		{
			throw new NotImplementedException();
		}

		public Tag[] GetTags()
		{
			throw new NotImplementedException();
		}

		public Book GetBook(int id)
		{
			throw new NotImplementedException();
		}

		public Category GetCategory(int id)
		{
			throw new NotImplementedException();
		}

		public Tag GetTag(int id)
		{
			throw new NotImplementedException();
		}

		public Book AddBook(Book book)
		{
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