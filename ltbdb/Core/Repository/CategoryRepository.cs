using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core
{
	public class CategoryRepository: Repository<Category>
	{
		public CategoryRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		public override Category Add(Category item)
		{
			throw new NotImplementedException();
		}

		public override Category Get(object id)
		{
			throw new NotImplementedException();
		}

		public override IQueryable<Category> GetAll()
		{
			throw new NotImplementedException();
		}

		public override void Update(Category item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(Category item)
		{
			throw new NotImplementedException();
		}
	}
}