using AutoMapper;
using ltbdb.DomainServices;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	//public class dboBook
	//{
	//	public int BookId { get; set; }
	//	public int Number { get; set; }
	//	public string Name { get; set; }
	//	public string Category { get; set; }
	//	public int CategoryId { get; set; }
	//}

    public class BookController : Controller
    {
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HandleError(View="Error")]
		[HttpGet]
		public ActionResult View(int? id)
		{
			var result = new Store().GetBook(id ?? 0);

			var book = Mapper.Map<BookModel>(result);

			var view = new BookViewDetailModel { Book = book };
			
			return View(view);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View("Edit");
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			return View();
		}
    }
}
