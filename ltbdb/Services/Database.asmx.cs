using AutoMapper;
using ltbdb.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace ltbdb.Services
{
	/// <summary>
	/// Simple webservice to retrieve items from database.
	/// </summary>
	[WebService(Namespace = "http://stichelbiene.de/ltbdb2/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)] 
	[System.Web.Script.Services.ScriptService]
	public class Database : System.Web.Services.WebService
	{
		[WebMethod(Description="List all available books.")]
		public ltbdb.Models.WebService.Book[] ListBooks()
		{
			return Mapper.Map<ltbdb.Models.WebService.Book[]>(Book.Get());
		}

		[WebMethod(Description = "Get a specified book by id.")]
		public ltbdb.Models.WebService.Book GetBook(int id)
		{
			return Mapper.Map<ltbdb.Models.WebService.Book>(Book.Get(id));
		}

		[WebMethod(Description = "List all available categories.")]
		public ltbdb.Models.WebService.Category[] ListCategories()
		{
			return Mapper.Map<ltbdb.Models.WebService.Category[]>(Category.Get());
		}

		[WebMethod(Description = "Get a specified category by id.")]
		public ltbdb.Models.WebService.Category GetCategory(int id)
		{
			return Mapper.Map<ltbdb.Models.WebService.Category>(Category.Get(id));
		}

		[WebMethod(Description = "List all available tags.")]
		public ltbdb.Models.WebService.Tag[] ListTags()
		{
			return Mapper.Map<ltbdb.Models.WebService.Tag[]>(Tag.Get());
		}

		[WebMethod(Description = "Get a specified tag by id.")]
		public ltbdb.Models.WebService.Tag GetTag(int id)
		{
			return Mapper.Map<ltbdb.Models.WebService.Tag>(Tag.Get(id));
		}
	}
}
