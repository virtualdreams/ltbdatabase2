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
			throw new NotImplementedException();
		}

		public override Tag2BookDTO Get(object id)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Tag2BookDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getTags2Book");

			var result = this.Context.QueryForList<Tag2BookDTO>(query);

			return result;
		}

		public override Tag2BookDTO Update(Tag2BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override bool Delete(Tag2BookDTO item)
		{
			throw new NotImplementedException();
		}
	}
}