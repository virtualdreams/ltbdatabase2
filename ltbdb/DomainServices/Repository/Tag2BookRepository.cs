using ltbdb.DomainServices.DTO;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices.Repository
{
	public class Tag2BookRepository: Repository<Tag2BookDTO>
	{
		public Tag2BookRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		static public Tag2BookDTO Default()
		{
			return new Tag2BookDTO { Id = 0, BookId = 0, TagId = 0 };
		}

		public override Tag2BookDTO Add(Tag2BookDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("addTag2Book");
			query.SetParameter("tagid", item.TagId);
			query.SetParameter("bookid", item.BookId);
			int id = 0;

			try
			{
				this.Context.BeginTransaction();
				var result = this.Context.Insert(query);
				id = this.GetLastInsertId();
				this.Context.CommitTransaction();
			}
			catch (Exception)
			{
				this.Context.RollbackTransaction();
				throw;
			}

			return Get(id);
		}

		public override Tag2BookDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getTag2Book");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObject<Tag2BookDTO>(query);

			return result;
		}

		public override IEnumerable<Tag2BookDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getTags2Book");

			var result = this.Context.QueryForObjectList<Tag2BookDTO>(query);

			return result;
		}

		public override Tag2BookDTO Update(Tag2BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override bool Delete(Tag2BookDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("deleteTag2Book");
			query.SetParameter<Tag2BookDTO>(item);

			return this.Context.Delete(query) > 0; 
		}
	}
}