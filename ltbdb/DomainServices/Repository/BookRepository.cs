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

		public override BookDTO Add(BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override BookDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getBook");
			query.SetEntity("id", id);

			var result = this.Context.QueryForObject<BookDTO>(query);

			return result;
		}

		public override IEnumerable<BookDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getBooks");

			var result = this.Context.QueryForList<BookDTO>(query);

			return result;
		}

		public override void Update(BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(BookDTO item)
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

		#endregion
	}
}