using AutoMapper;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ltbdb.DomainServices;
using ltbdb.DomainServices.DTO;
using ltbdb.Models;
using ltbdb.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ltbdb
{
	// Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
	// finden Sie unter "http://go.microsoft.com/?LinkId=9394801".
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			MvcApplication.BootstrapWindsor();
			MvcApplication.BootstrapAutoMapper();
		}

		public static IWindsorContainer Container;
		protected void Application_End()
		{
			Container.Dispose();
		}

		private static void BootstrapWindsor()
		{
			Container = new WindsorContainer().Install(FromAssembly.This());

			var controllerFactory = new WindsorControllerFactory(Container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}

		private static void BootstrapAutoMapper()
		{
			// Repository -> Domain

			Mapper.CreateMap<BookDTO, Book>()
				.ForMember(d => d.Created, map => map.MapFrom(s => s.Added))
				.ForMember(d => d.Category, map => map.MapFrom(s => new Category { Id = s.Category, Name = s.CategoryName }));

			Mapper.CreateMap<CategoryDTO, Category>();

			Mapper.CreateMap<TagDTO, Tag>()
				.ForMember(d => d.References, map => map.MapFrom(s => s.Ref));
			
			Mapper.CreateMap<StoryDTO, Story>();

			//Domain -> View

			Mapper.CreateMap<Book, BookModel>()
				.ForMember(s => s.Category, map => map.MapFrom(d => d.Category.Id))
				.ForMember(s => s.CategoryName, map => map.MapFrom(d => d.Category.Name));

			Mapper.CreateMap<Tag, TagModel>();

			Mapper.CreateMap<Category, CategoryModel>();

			Mapper.CreateMap<Story, StoryModel>();
		}
	}
}