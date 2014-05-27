using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core
{
	public class BookRepository: Repository<Book>
	{
		public BookRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		public override Book Add(Book item)
		{
			SqlQuery query = Config.CreateQuery("insertBook");
			query.SetInt("number", item.Number);
			query.SetString("name", item.Name);
			query.SetInt("category", item.Category.Id);

			Context.BeginTransaction();
			try
			{
				Context.Insert(query);
				int id = Context.QueryForScalar<int>(Config.CreateQuery("getLastInsertId"));
				Context.CommitTransaction();

				item.Id = id;
			}
			catch(Exception)
			{
				Context.RollbackTransaction();
			}

			return item;
		}

		public override Book Get(object id)
		{
			throw new NotImplementedException();
		}

		public override IQueryable<Book> GetAll()
		{
			throw new NotImplementedException();
		}

		public override void Update(Book item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(Book item)
		{
			throw new NotImplementedException();
		}
	}
}