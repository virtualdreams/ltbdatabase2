using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.Core.Filter;
using ltbdb.Core.Helpers;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order=99)]
    public class TagController : Controller
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
		public ActionResult Index()
		{
			var _tags = Tag.Get();
			var _referencedTags = _tags.Where(s => s.References != 0).OrderBy(o => o.Name);
			var _unreferencedTags = _tags.Where(s => s.References == 0).OrderBy(o => o.Name);

			var referencedTags = Mapper.Map<TagModel[]>(_referencedTags);
			var unreferencedTags = Mapper.Map<TagModel[]>(_unreferencedTags);

			var view = new TagViewContainer
			{
				Tags = referencedTags,
				UnreferencedTags = unreferencedTags
			};

			return View(view);
		}

		[HttpGet]
        public ActionResult View(string id, int? ofs)
        {
			var _tag = Tag.GetByName(id);
			if (_tag == null)
				throw new HttpException(404, "Resource not found.");

			var _books = Book.GetByTag(_tag.Id);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var tag = Mapper.Map<TagModel>(_tag);
			var books = Mapper.Map<BookModel[]>(_books);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewTagContainer
			{
				Books = books,
				Tag = tag,
				PageOffset = offset
			};

			return View(view);
        }

		[Authorize]
		[HttpGet]
		public ActionResult Edit(int? id)
		{
			var _tag = Tag.Get(id ?? 0);
			if(_tag == null)
				throw new HttpException(404, "Resource not found.");

			var tag = Mapper.Map<TagModel>(_tag);

			var view = new TagEditContainer { Tag = tag };

			return View("edit", view);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Edit(TagModel model)
		{
			if (!ModelState.IsValid)
			{
				var view = new TagEditContainer { Tag = model };

				return View("edit", view);
			}

			var tag = Mapper.Map<Tag>(model);

			Tag.Update(tag);

			return RedirectToAction("index", "tag");
		}
    }
}
