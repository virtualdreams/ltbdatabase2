using ltbdb.DomainServices.DTO;
using SqlDataMapper;
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
			query.SetEntities<CategoryDTO>(item);
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
			query.SetEntity("id", id);

			var result = this.Context.QueryForObject<CategoryDTO>(query);

			return result ?? Default();
		}

		public override IEnumerable<CategoryDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getCategories");

			var result = this.Context.QueryForList<CategoryDTO>(query);

			return result;
		}

		public override CategoryDTO Update(CategoryDTO item)
		{
			SqlQuery query = this.Config.CreateQuery("updateCategory");
			query.SetEntities<CategoryDTO>(item);

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
			throw new NotImplementedException();
		}
	}
}