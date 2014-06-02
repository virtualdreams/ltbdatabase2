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

		public override CategoryDTO Add(CategoryDTO item)
		{
			throw new NotImplementedException();
		}

		public override CategoryDTO Get(object id)
		{
			SqlQuery query = this.Config.CreateQuery("getCategory");
			query.SetEntity("id", id);

			var result = this.Context.QueryForObject<CategoryDTO>(query);

			return result ?? new CategoryDTO { Id = 0, Name = "" };
		}

		public override IEnumerable<CategoryDTO> GetAll()
		{
			SqlQuery query = this.Config.CreateQuery("getCategories");

			var result = this.Context.QueryForList<CategoryDTO>(query);

			return result;
		}

		public override void Update(CategoryDTO item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(CategoryDTO item)
		{
			throw new NotImplementedException();
		}
	}
}