using AutoMapper;
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
			
			//dboBook book = new dboBook { BookId = 211, Category = "xxx", CategoryId = 2, Name = "yyyy", Number = 231 };
			//string[] c = new string[] { "xxxx", "yyy", "zzzz" };
			
			// mapping stuff
			//var mapper = Mapper.CreateMap<dboBook, BookDetailModel>();
			//mapper.ForMember(d => d.Id, map => map.MapFrom(s => s.BookId));

			//var mapper2 = Mapper.CreateMap<string[], BookDetailModel>();
			//mapper2.ForMember(d => d.Categories, map => map.MapFrom(s => s));

			//Mapper.AssertConfigurationIsValid();

			// mapping here
			//BookDetailModel output = Mapper.Map<BookDetailModel>(book);
			//Mapper.Map(c, output);

			BookDetailModel view = new BookDetailModel { Name = "Der Kolumbusfalter", Id = 1, Number = 1, Category = "Lustiges Taschenbuch" };
			
			return View(view);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			return View();
		}
    }
}
