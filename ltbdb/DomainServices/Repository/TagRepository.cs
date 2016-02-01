using ltbdb.DomainServices.DTO;
using SqlDataMapper;
using SqlDataMapper.Extension;
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
			query.SetParameter<TagDTO>(item);
			int id = 0;

			try
			{
				this.Context.Insert(query);
				id = this.GetLastInsertId();
			}
			catch (Exception)
			{
				throw;
			}

			return Get(id);
		}

		public override TagDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getTagById");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObject<TagDTO>(query);

			return result ?? Default();
		}

		public override IEnumerable<TagDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getTags");

			var result = this.Context.QueryForObjectList<TagDTO>(query);

			return result;
		}

		public override TagDTO Update(TagDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("updateTag");
			query.SetParameter<TagDTO>(item);

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

		public override bool Delete(TagDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("deleteTag");
			query.SetParameter<TagDTO>(item);

			return this.Context.Delete(query) > 0;
		}

		#region Additional

		public IEnumerable<TagDTO> GetByBook(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getTagsByBook");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObjectList<TagDTO>(query);

			return result;
		}

		public TagDTO GetByName(string name)
		{
			SqlQuery query = this.Config.CreateQuery("getTagByName");
			query.SetParameter("name", name);

			var result = this.Context.QueryForObject<TagDTO>(query);

			return result;
		}

		#endregion
	}
}