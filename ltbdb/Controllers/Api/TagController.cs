using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[LogError(Order = 0)]
	public class TagController : ApiController
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TagController));

		private readonly BookService Book;
		private readonly TagService Tag;
		private readonly CategoryService Category;

		public TagController(BookService book, TagService tag, CategoryService category)
		{
			Book = book;
			Tag = tag;
			Category = category;
		}

		[HttpGet]
		public IEnumerable<dynamic> Get()
		{
			var _tags = Tag.Get().OrderBy(o => o.Name);

			foreach (var tag in _tags)
			{
				yield return new
				{
					Id = tag.Id,
					Name = tag.Name,
					References = tag.References
				};
			}
		}

		[HttpGet]
		public dynamic Get(int id)
		{
			var _tag = Tag.Get(id);
			if (_tag == null)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found."));

			return new
			{
				Id = _tag.Id,
				Name = _tag.Name,
				References = _tag.References
			};
		}

		[Authorize]
		[HttpPost]
		public IEnumerable<dynamic> Add([FromBody]AddTagModel model)
		{
			var _book = Book.Get(model.Id);
			if(_book == null)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found."));

			var _tags = Tag.Add(_book.Id, (model.Tags ?? String.Empty).Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(w => !String.IsNullOrEmpty(w)).ToArray());
			foreach (var tag in _tags)
			{
				yield return new
				{
					Id = tag.Id,
					Name = tag.Name,
					Book = model.Id
				};
			}
		}

		[Authorize]
		[HttpPost]
		public dynamic Remove([FromBody]RemoveTagModel model)
		{
			var _result = Tag.Remove(model.Id, model.TagId);

			return new
			{
				success = _result
			};
		}

		[Authorize]
		[HttpPost]
		public dynamic Delete(int id)
		{
			var _tag = Tag.Get(id);

			if (_tag == null)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Id not found."));

			var _result = Tag.Delete(_tag);

			return new
			{
				success = _result
			};
		}
	}
}
