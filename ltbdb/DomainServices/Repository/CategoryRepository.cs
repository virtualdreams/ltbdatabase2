using ltbdb.DomainServices.DTO;
using SqlDataMapper;
using SqlDataMapper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices.Repository
{
	public class CategoryRepository: Repository<CategoryDTO>
	{
		public CategoryRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		static public CategoryDTO Default()
		{
			return new CategoryDTO { Id = 0, Name = "" };
		}

		public override CategoryDTO Add(CategoryDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("addCategory");
			query.SetParameter<CategoryDTO>(item);
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

		public override CategoryDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getCategory");
			query.SetParameter("id", id);

			var result = this.Context.QueryForObject<CategoryDTO>(query);

			return result ?? Default();
		}

		public override IEnumerable<CategoryDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getCategories");

			var result = this.Context.QueryForObjectList<CategoryDTO>(query);

			return result;
		}

		public override CategoryDTO Update(CategoryDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("updateCategory");
			query.SetParameter<CategoryDTO>(item);

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

		public override bool Delete(CategoryDTO item)
		{
			var query = this.Config.CreateQuery("deleteCategory");
			query.SetParameter("id", item.Id);

			try
			{
				var ret = this.Context.Delete(query);

				return ret > 0;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public bool Move(CategoryDTO from, CategoryDTO to)
		{
			var query = this.Config.CreateQuery("moveBooks");
			query.SetParameter("from", from.Id);
			query.SetParameter("to", to.Id);

			try
			{
				var ret = this.Context.Update(query);

				return ret > 0;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}