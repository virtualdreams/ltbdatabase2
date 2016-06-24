using log4net;
using ltbdb.Core.Database.Repository;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Database
{
	///// <summary>
	///// Access the database. Provide entities for each table.
	///// </summary>
	//public class DatabaseContext
	//{
	//	private static readonly ILog Log = LogManager.GetLogger(typeof(DatabaseContext));

	//	public SqlConfig SqlConfig { get; private set; }
	//	public SqlContext SqlContext { get; private set; }

	//	public BookRepository BookEntity { get; private set; }
	//	public CategoryRepository CategoryEntity { get; private set; }
	//	public TagRepository TagEntity { get; private set; }
	//	public Tag2BookRepository Tag2BookEntity { get; private set; }

	//	public DatabaseContext()
	//	{
	//		// get config and context from request
	//		this.SqlConfig = HttpContext.Current.Items["config"] as SqlConfig;
	//		this.SqlContext = HttpContext.Current.Items["context"] as SqlContext;

	//		BookEntity = new BookRepository(this.SqlConfig, this.SqlContext);
	//		CategoryEntity = new CategoryRepository(this.SqlConfig, this.SqlContext);
	//		TagEntity = new TagRepository(this.SqlConfig, this.SqlContext);
	//		Tag2BookEntity = new Tag2BookRepository(this.SqlConfig, this.SqlContext);
	//	}
	//}

	public class DatabaseContextNew
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(DatabaseContextNew));

		protected SqlConfig SqlConfig { get; private set; }
		protected SqlContext SqlContext { get; private set; }

		protected BookRepository BookEntity { get; private set; }
		protected CategoryRepository CategoryEntity { get; private set; }
		protected TagRepository TagEntity { get; private set; }
		protected Tag2BookRepository Tag2BookEntity { get; private set; }

		public DatabaseContextNew(SqlConfig config, SqlContext context)
		{
			// get config and context from request
			//this.SqlConfig = HttpContext.Current.Items["config"] as SqlConfig;
			//this.SqlContext = HttpContext.Current.Items["context"] as SqlContext;

			this.SqlConfig = config;
			this.SqlContext = context;

			BookEntity = new BookRepository(this.SqlConfig, this.SqlContext);
			CategoryEntity = new CategoryRepository(this.SqlConfig, this.SqlContext);
			TagEntity = new TagRepository(this.SqlConfig, this.SqlContext);
			Tag2BookEntity = new Tag2BookRepository(this.SqlConfig, this.SqlContext);
		}
	}
}