using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core
{
	public class LtbStore
	{
		public LtbStore()
		{

		}

		public Book AddBook(Book book)
		{
			SqlConfig config = new SqlConfig();
			SqlContext context = config.CreateContext();

			if (book.Category == null)
			{
				throw new Exception("Category must not null");
			}

			SqlQuery query = config.CreateQuery("insertBook");
			query.SetInt("number", book.Number);
			query.SetString("name", book.Name);
			query.SetInt("category", book.Category.Id);

			context.BeginTransaction();
			try
			{
				context.Insert(query);
				int id = context.QueryForScalar<int>(config.CreateQuery("getLastInsertId"));
				context.CommitTransaction();

				book.Id = id;
			}
			catch (Exception)
			{
				context.RollbackTransaction();
			}

			return book;
		}

		public Book GetBook(int id)
		{
			return new Book();
		}

		public Book[] GetBooks()
		{
			return new Book[] { };
		}

		public Book[] GetBooks(string search)
		{
			return new Book[] { };
		}
	}
}