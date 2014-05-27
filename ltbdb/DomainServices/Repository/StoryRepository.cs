using ltbdb.DomainServices.DTO;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices.Repository
{
	public class StoryRepository: Repository<StoryDTO>
	{
		public StoryRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		public override StoryDTO Add(StoryDTO item)
		{
			throw new NotImplementedException();
		}

		public override StoryDTO Get(object id)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<StoryDTO> GetAll()
		{
			throw new NotImplementedException();
		}

		public override void Update(StoryDTO item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(StoryDTO item)
		{
			throw new NotImplementedException();
		}
	}
}