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

			var books = bookRepo.GetAll();

			var mapper = Mapper.CreateMap<BookDTO, Book>();
			mapper.ForMember(d => d.Created, map => map.MapFrom(s => s.Added));
			mapper.ForMember(d => d.Category, map => map.MapFrom(s => new Category { Id = s.Category, Name = s.CategoryName }));

			var result = Mapper.Map<IEnumerable<BookDTO>, Book[]>(books);

			return result;
		}

		/// <summary>
		/// Get a specified book.
		/// </summary>
		/// <param name="book">Book object with filled Id</param>
		/// <returns>Book</returns>
		public Book GetBook(Book book)
		{
			return GetBook(book.Id);
		}

		/// <summary>
		/// Get a specified book.
		/// </summary>
		/// <param name="id">Id of the book</param>
		/// <returns>Book</returns>
		public Book GetBook(int id)
		{
			BookRepository bookRepo = new BookRepository(this.Config, this.Context);

			var book = bookRepo.Get(id);

			var mapper = Mapper.CreateMap<BookDTO, Book>();
			mapper.ForMember(d => d.Created, map => map.MapFrom(s => s.Added));
			mapper.ForMember(d => d.Category, map => map.MapFrom(s => new Category { Id = s.Category, Name = s.CategoryName }));

			var result = Mapper.Map<Book>(book);

			return result;
		}

		/// <summary>
		/// Get the 12 recently added books.
		/// </summary>
		/// <returns>List of books</returns>
		public Book[] GetRecentlyAdded()
		{
			return GetBooks().OrderByDescending(o => o.Created).Take(12).ToArray();
		}

		/// <summary>
		/// Get all catgeories
		/// </summary>
		/// <returns>List of categories</returns>
		public Category[] GetCategories()
		{
			CategoryRepository catRepo = new CategoryRepository(this.Config, this.Context);

			var categories = catRepo.GetAll();
			
			var mapper = Mapper.CreateMap<CategoryDTO, Category>();

			var result = Mapper.Map<IEnumerable<CategoryDTO>, Category[]>(categories);

			return result;
		}

		/// <summary>
		/// Get a specified category.
		/// </summary>
		/// <param name="category">Category object with filled id.</param>
		/// <returns>Category</returns>
		public Category GetCategory(Category category)
		{
			return GetCategory(category.Id);
		}

		/// <summary>
		/// Get a specified category.
		/// </summary>
		/// <param name="id">Id of the categoy</param>
		/// <returns>Category</returns>
		public Category GetCategory(int id)
		{
			CategoryRepository catRepo = new CategoryRepository(this.Config, this.Context);

			var category = catRepo.Get(id);

			var mapper = Mapper.CreateMap<CategoryDTO, Category>();

			var result = Mapper.Map<Category>(category);

			return result;
		}

		/// <summary>
		/// Get all tags.
		/// </summary>
		/// <returns></returns>
		public Tag[] GetTags()
		{
			TagRepository tagRepo = new TagRepository(this.Config, this.Context);

			var tags = tagRepo.GetAll();

			var mapper = Mapper.CreateMap<TagDTO, Tag>();

			var result = Mapper.Map<IEnumerable<TagDTO>, Tag[]>(tags);

			return result;
		}

		/// <summary>
		/// Get a specified tag.
		/// </summary>
		/// <param name="id">The tag id.</param>
		/// <returns>Tag</returns>
		public Tag GetTag(int id)
		{
			TagRepository tagRepo = new TagRepository(this.Config, this.Context);

			var tag = tagRepo.Get(id);

			var mapper = Mapper.CreateMap<TagDTO, Tag>();

			var result = Mapper.Map<Tag>(tag);

			return result;
		}

		public Book[] Search(string term)
		{
			return new Book[] { };
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