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

		static public TagDTO Default()
		{
			return new TagDTO { Id = 0, Name = "" };
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

			return result ?? Default();
		}

		public override IEnumerable<TagDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getTags");

			var result = this.Context.QueryForList<TagDTO>(query);

			return result;
		}

		public override TagDTO Update(TagDTO item)
		{
			throw new NotImplementedException();
		}

		public override bool Delete(TagDTO item)
		{
			throw new NotImplementedException();
		}

		#region Additional

		public IEnumerable<TagDTO> GetByBook(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getTagsByBook");
			query.SetEntity("id", id);

			var result = this.Context.QueryForList<TagDTO>(query);

			return result;
		}

		#endregion
	}
}