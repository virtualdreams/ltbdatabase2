using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Core.Helpers
{
	/// <summary>
	/// Custom model binder for MongoDB ObjectId.
	/// </summary>
	public class ObjectIdModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (result == null)
			{
				return ObjectId.Empty;
			}

			ObjectId _id;
			if (!ObjectId.TryParse((string)result.ConvertTo(typeof(string)), out _id))
				return ObjectId.Empty;

			return _id;
		}
	}
}