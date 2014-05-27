using ltbdb.DomainServices.DTO;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices.Repository
{
	public class TagRepository: Repository<TagDTO>
	{
		public TagRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		public override TagDTO Add(TagDTO item)
		{
			throw new NotImplementedException();
		}

		public override TagDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getTag");
			query.SetEntity("id", id);

			var result = this.Context.QueryForObject<TagDTO>(query);

			return result;
		}

		public override IEnumerable<TagDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getTags");

			var result = this.Context.QueryForList<TagDTO>(query);

			return result;
		}

		public override void Update(TagDTO item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(TagDTO item)
		{
			throw new NotImplementedException();
		}
	}
}