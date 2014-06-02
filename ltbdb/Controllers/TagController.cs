using AutoMapper;
using Castle.Core.Logging;
using ltbdb.DomainServices;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
    public class TagController : Controller
    {
		public ILogger log { get; set; }

        public ActionResult View(int? id)
        {
			var _tag = new Store().GetTag(id ?? 0);

			var _books = _tag.GetBooks();

			var tag = Mapper.Map<TagModel>(_tag);
			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewTagModel { Books = books, Tag = tag };

			return View(view);
        }
    }
}
