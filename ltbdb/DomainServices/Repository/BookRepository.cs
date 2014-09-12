using ltbdb.DomainServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

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
			query.SetInt("category", item.Category);
			query.SetInt("number", item.Number);
			query.SetString("name", item.Name);

			var result = this.Context.Insert(query);

			item.Id = this.GetLastInsertId();
			
			return item;
		}

		public override BookDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBook");
			query.SetEntity("id", id);

			var result = this.Context.QueryForObject<BookDTO>(query);

			return result ?? Default();
		}

		public override IEnumerable<BookDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getBooks");

			var result = this.Context.QueryForList<BookDTO>(query);

			return result;
		}

		public override BookDTO Update(BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override bool Delete(BookDTO item)
		{
			throw new NotImplementedException();
		}

		#region Additional

		public IEnumerable<BookDTO> GetByTag(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBooksByTag");
			query.SetEntity("id", id);

			var result = this.Context.QueryForList<BookDTO>(query);

			return result;
		}

		public IEnumerable<BookDTO> GetByCategory(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBooksByCategory");
			query.SetEntity("id", id);

			var result = this.Context.QueryForList<BookDTO>(query);

			return result;
		}

		public IEnumerable<BookDTO> GetByTerm(string term)
		{
			SqlQuery query = this.Config.CreateQuery("getBookByTerm");
			query.SetString("term", String.Format("%{0}%", term));

			var result = this.Context.QueryForList<BookDTO>(query);

			return result;
		}

		public IEnumerable<string> GetSuggestionList(string term)
		{
			SqlQuery query = this.Config.CreateQuery("getSuggestionList");
			query.SetString("term", String.Format("%{0}%", term));

			var result = this.Context.QueryForScalarList<string>(query);

			return result;
		}

		#endregion
	}
}