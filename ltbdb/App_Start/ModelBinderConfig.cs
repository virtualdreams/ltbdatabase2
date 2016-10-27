using ltbdb.Core.Helpers;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb
{
	public class ModelBinderConfig
	{
		static public void RegisterModelBinder()
		{
			ModelBinders.Binders.Add(typeof(ObjectId), new ObjectIdModelBinder());
		}
	}
}