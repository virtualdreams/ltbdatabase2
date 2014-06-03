using Castle.Core.Logging;
using ltbdb.DomainServices.Repository;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class DatabaseContext
	{
		protected ILogger Log { get; set; }

		protected SqlConfig SqlConfig { get; private set; }
		protected SqlContext SqlContext { get; private set; }

		protected BookRepository BookEntity { get; private set; }
		protected CategoryRepository CategoryEntity { get; private set; }
		protected TagRepository TagEntity { get; private set; }
		protected Tag2BookRepository Tag2BookEntity { get; private set; }
		protected StoryRepository StoryEntity { get; private set; }

		public DatabaseContext()
		{
			this.Log = MvcApplication.Container.Resolve<ILogger>();
			
			this.SqlConfig = HttpContext.Current.Items["config"] as SqlConfig;
			this.SqlContext = HttpContext.Current.Items["context"] as SqlContext;

			BookEntity = new BookRepository(this.SqlConfig, this.SqlContext);
			CategoryEntity = new CategoryRepository(this.SqlConfig, this.SqlContext);
			TagEntity = new TagRepository(this.SqlConfig, this.SqlContext);
			Tag2BookEntity = new Tag2BookRepository(this.SqlConfig, this.SqlContext);
			StoryEntity = new StoryRepository(this.SqlConfig, this.SqlContext);
		}
	}
}