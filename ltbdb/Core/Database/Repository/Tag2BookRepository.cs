using log4net;
using ltbdb.Core.Database.DTO;
using SqlDataMapper;
using SqlDataMapper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Database.Repository
{
	public class Tag2BookRepository: Repository<Tag2BookDTO>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Tag2BookRepository));

		public Tag2BookRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
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

			if (Log.IsDebugEnabled)
			{
				Log.Debug(query.QueryString);
			}

			return this.Context.Delete(query) > 0; 
		}
	}
}