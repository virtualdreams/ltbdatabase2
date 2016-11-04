﻿using log4net;
using ltbdb.Core.Helpers;
using ltbdb.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ltbdb.Core.Services
{
	public class BookService: MongoContext
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BookService));

		/// <summary>
		/// Initializes the BookService class.
		/// </summary>
		/// <param name="client"></param>
		public BookService(IMongoClient client)
			: base(client)
		{ }

		/// <summary>
		/// Get all book from storage.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Book> Get()
		{
			var _filter = Builders<Book>.Filter;
			var _all = _filter.Empty;

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(o => o.Number).Ascending(o => o.Category);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_all).Sort(_order).ToString());
			}

			return Book.Find(_all).Sort(_order).ToEnumerable();
		}

		/// <summary>
		/// Get book by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Book GetById(ObjectId id)
		{
			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, id);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_id).ToString());
			}

			return Book.Find(_id).SingleOrDefault();
		}

		/// <summary>
		/// Get books by category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public IEnumerable<Book> GetByCategory(string category)
		{
			category = category.Trim();

			var _filter = Builders<Book>.Filter;
			var _category = _filter.Eq(f => f.Category, category);

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Number);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_category).Sort(_order).ToString());
			}

			return Book.Find(_category).Sort(_order).ToEnumerable();
		}

		/// <summary>
		/// Get books by tag.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public IEnumerable<Book> GetByTag(string tag)
		{
			tag = tag.Trim();

			var _filter = Builders<Book>.Filter;
			var _tag = _filter.AnyIn("Tags", new string[] { tag });

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Number).Ascending(f => f.Category);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_tag).Sort(_order).ToString());
			}

			return Book.Find(_tag).Sort(_order).ToEnumerable();
		}

		/// <summary>
		/// Get recently added books.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Book> GetRecentlyAdded()
		{
			var _filter = Builders<Book>.Filter;
			var _all = _filter.Empty;

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Descending(o => o.Created);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_all).Sort(_order).ToString());
			}

			return Book.Find(_all).Sort(_order).Limit(GlobalConfig.Get().RecentItems).ToEnumerable();
		}

		/// <summary>
		/// Search for books.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public IEnumerable<Book> Search(string term)
		{
			term = term.Trim();

			// TODO search in number, title and in stories
			var _filter = Builders<Book>.Filter;
			var _title = _filter.Regex(f => f.Title, new BsonRegularExpression(Regex.Escape(term), "i"));
			var _stories = _filter.Regex("Stories", new BsonRegularExpression(Regex.Escape(term), "i"));

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Number).Ascending(f => f.Title);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_title | _stories).Sort(_order).ToString());
			}

			return Book.Find(_title | _stories).Sort(_order).ToEnumerable();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of categories.</returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			var _filter = Builders<Book>.Filter;
			var _title = _filter.Regex(f => f.Title, new BsonRegularExpression(Regex.Escape(term), "i"));

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Title);

			if (Log.IsDebugEnabled)
			{
				Log.Debug(Book.Find(_title).Sort(_order).ToString());
			}

			return Book.Find(_title).Sort(_order).ToEnumerable().Select(s => s.Title);
		}

		/// <summary>
		/// Create a new book.
		/// </summary>
		/// <param name="book"></param>
		public ObjectId Create(Book book)
		{
			book.Filename = null;
			book.Created = DateTime.Now;

			Book.InsertOne(book);

			if (book.Id == ObjectId.Empty)
			{
				Log.ErrorFormat("Insert new book failed.");
				return ObjectId.Empty;
			}
			else
			{
				Log.InfoFormat("Insert new book with id '{0}'.", book.Id);
				return book.Id;
			}
		}

		/// <summary>
		/// Update an existing book.
		/// </summary>
		/// <param name="book"></param>
		public ObjectId Update(Book book)
		{
			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, book.Id);

			var _update = Builders<Book>.Update;
			var _set = _update
				.Set(s => s.Number, book.Number)
				.Set(s => s.Title, book.Title)
				.Set(s => s.Category, book.Category)
				.Set(s => s.Stories, book.Stories)
				.Set(s => s.Tags, book.Tags);

			var _result = Book.UpdateOne(_id, _set);
			if (_result.IsAcknowledged && _result.MatchedCount > 0)
			{
				Log.InfoFormat("Update book '{0}'", book.Id);
				return book.Id;
			}
			else
			{
				Log.ErrorFormat("Update book '{0}' failed.", book.Id);
				return ObjectId.Empty;
			}
		}

		/// <summary>
		/// Delete an existing book.
		/// </summary>
		/// <param name="id"></param>
		public bool Delete(ObjectId id)
		{
			var _book = GetById(id);

			if (_book == null)
				return false;

			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, _book.Id);

			var _result = Book.DeleteOne(_id);
			if (_result.IsAcknowledged && _result.DeletedCount != 0)
			{
				RemoveImage(_book.Filename);

				Log.InfoFormat("Delete book '{0}'", id);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Set the cover for the book. If stream is null, the image get removed.
		/// </summary>
		/// <param name="id">The id of the book.</param>
		/// <param name="stream">The image stream.</param>
		/// <returns>True on success.</returns>
		public bool SetImage(ObjectId id, Stream stream)
		{
			var _book = GetById(id);

			if (_book == null)
				return false;

			if (stream == null)
			{
				// remove the old images
				RemoveImage(_book.Filename);
				_book.Filename = null;
			}
			else
			{
				// remove the old images and store the new one
				RemoveImage(_book.Filename);
				var _filename = ImageStore.Save(stream, true);
				if (String.IsNullOrEmpty(_filename))
					return false;

				_book.Filename = _filename;
			}

			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, id);

			var _update = Builders<Book>.Update;
			var _set = _update.Set(f => f.Filename, _book.Filename);

			var _result = Book.UpdateOne(_id, _set);

			if (_result.IsAcknowledged && _result.ModifiedCount != 0)
			{
				Log.InfoFormat("Update filename for book '{0}'", id);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Remove image from storage.
		/// </summary>
		/// <param name="filename">The filename.</param>
		private void RemoveImage(string filename)
		{
			ImageStore.Remove(filename, true);
		}
	}
}