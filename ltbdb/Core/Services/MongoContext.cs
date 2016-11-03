﻿using ltbdb.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Services
{
	public class MongoContext
	{
		/// <summary>
		/// The mongo client.
		/// </summary>
		protected readonly IMongoClient _client;
		
		/// <summary>
		/// The mongo database.
		/// </summary>
		protected readonly IMongoDatabase _db;

		/// <summary>
		/// The book collection.
		/// </summary>
		protected IMongoCollection<Book> Book { get; private set; }

		///// <summary>
		///// The full sized images collection.
		///// </summary>
		//protected GridFSBucket Images { get; private set; }

		///// <summary>
		///// The thumbs images collection.
		///// </summary>
		//protected GridFSBucket Thumbs { get; private set; }

		public MongoContext(IMongoClient client)
		{
			_client = client;
			_db = _client.GetDatabase("ltbdb"); // TODO database name configureable
			Book = _db.GetCollection<Book>("book");
			//Images = new GridFSBucket(_db, new GridFSBucketOptions { BucketName = "images" });
			//Thumbs = new GridFSBucket(_db, new GridFSBucketOptions { BucketName = "thumbs" });
		}
	}
}