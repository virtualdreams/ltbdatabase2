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
			SqlQuery query = this.Config.CreateQuery("addTag");
			query.SetString("name", item.Name);
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

		public override TagDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getTagById");
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

		public TagDTO GetByName(string name)
		{
			SqlQuery query = this.Config.CreateQuery("getTagByName");
			query.SetEntity("name", name);

			var result = this.Context.QueryForObject<TagDTO>(query);

			return result;
		}

		#endregion
	}
}