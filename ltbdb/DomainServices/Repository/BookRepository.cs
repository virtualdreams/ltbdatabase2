using ltbdb.DomainServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;
using SqlDataMapper.Extension;

namespace ltbdb.DomainServices.Repository
{
	public class BookRepository: Repository<BookDTO>
	{
		public BookRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		/// <summary>
		/// Represents a default object.
		/// </summary>
		/// <returns></returns>
		static public BookDTO Default()
		{
			return new BookDTO { Id = 0, Category = 0, CategoryName = "", Name = "", Number = 0, Added = DateTime.MinValue };
		}

		public override BookDTO Add(BookDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("addBook");
			query.SetParameter<BookDTO>(item);
			int id = 0;

			try
			{
				this.Context.Insert(query);
				id = this.GetLastInsertId();
			}
			catch(Exception)
			{
				throw;
			}

			return Get(id);
		}

		public override BookDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBook");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObject<BookDTO>(query);

			return result ?? Default();
		}

		public override IEnumerable<BookDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getBooks");

			var result = this.Context.QueryForObjectList<BookDTO>(query);

			return result;
		}

		public override BookDTO Update(BookDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("updateBook");
			query.SetParameter<BookDTO>(item);

			try
			{
				this.Context.Update(query);
			}
			catch (Exception)
			{
				throw;
			}

			return Get(item.Id);
		}

		public override bool Delete(BookDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("deleteBook");
			query.SetParameter<BookDTO>(item);

			return this.Context.Delete(query) > 0;
		}

		#region Additional

		public IEnumerable<BookDTO> GetByTag(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBooksByTag");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObjectList<BookDTO>(query);

			return result;
		}

		public IEnumerable<BookDTO> GetByCategory(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBooksByCategory");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObjectList<BookDTO>(query);

			return result;
		}

		public IEnumerable<BookDTO> GetByTerm(string term)
		{
			SqlQuery query = this.Config.CreateQuery("getBookByTerm");
			query.SetParameter("term", String.Format("%{0}%", term));

			var result = this.Context.QueryForObjectList<BookDTO>(query);

			return result;
		}

		public IEnumerable<string> GetSuggestionList(string term)
		{
			SqlQuery query = this.Config.CreateQuery("getSuggestionList");
			query.SetParameter("term", String.Format("%{0}%", term));

			var result = this.Context.QueryForScalarList<string>(query);

			return result;
		}

		public BookDTO UpdateImage(BookDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("updateBookImage");
			query.SetParameter<BookDTO>(item);

			try
			{
				this.Context.Update(query);
			}
			catch (Exception)
			{
				throw;
			}

			return Get(item.Id);
		}

		#endregion
	}
}