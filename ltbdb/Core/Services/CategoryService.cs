using log4net;
using ltbdb.Core.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Services
{
	public class CategoryService : MongoContext
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryService));

		public CategoryService(IMongoClient client)
			: base(client)
		{ }

		/// <summary>
		/// Get all available categories.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			return Book.Find(_ => true).ToEnumerable().Select(s => s.Category).Distinct();
		}

		/// <summary>
		/// Rename a category. Returns false if no document modified.
		/// </summary>
		/// <param name="from">The original category name.</param>
		/// <param name="to">The target category name.</param>
		/// <returns></returns>
		public bool Rename(string from, string to)
		{
			var _filter = Builders<Book>.Filter;
			var _from = _filter.Eq(f => f.Category, from);

			var _update = Builders<Book>.Update;
			var _set = _update.Set(s => s.Category, to);

			var _result = Book.UpdateMany(_from, _set);

			if (_result.IsAcknowledged && _result.ModifiedCount > 0)
			{
				Log.InfoFormat("Rename category '{0}' to '{1}'. Modified {2} documents.", from, to, _result.ModifiedCount);
				return true;
			}
			else
			{
				Log.ErrorFormat("Rename category '{0}' failed. No document was modified.", from);
				return false;
			}
		}
	}
}