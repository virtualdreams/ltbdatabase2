using log4net;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Services
{
	public class TagService: MongoContext
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TagService));

		public TagService(IMongoClient client)
			: base(client)
		{ }

		/// <summary>
		/// Get all available tags.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			return Book.Find(_ => true).ToEnumerable().SelectMany(s => s.Tags).Distinct();
		}
	}
}