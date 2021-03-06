﻿using log4net;
using ltbdb.DomainServices.Repository;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	/// <summary>
	/// Access the database. Provide entities for each table.
	/// </summary>
	public class Database
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Database));

		public SqlConfig SqlConfig { get; private set; }
		public SqlContext SqlContext { get; private set; }

		public BookRepository BookEntity { get; private set; }
		public CategoryRepository CategoryEntity { get; private set; }
		public TagRepository TagEntity { get; private set; }
		public Tag2BookRepository Tag2BookEntity { get; private set; }

		public Database()
		{
			// get config and context from request
			this.SqlConfig = HttpContext.Current.Items["config"] as SqlConfig;
			this.SqlContext = HttpContext.Current.Items["context"] as SqlContext;

			BookEntity = new BookRepository(this.SqlConfig, this.SqlContext);
			CategoryEntity = new CategoryRepository(this.SqlConfig, this.SqlContext);
			TagEntity = new TagRepository(this.SqlConfig, this.SqlContext);
			Tag2BookEntity = new Tag2BookRepository(this.SqlConfig, this.SqlContext);
		}
	}
}