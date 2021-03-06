﻿using AutoMapper;
using ltbdb.DomainServices;
using ltbdb.DomainServices.DTO;
using ltbdb.Models;
using ltbdb.Core;
using ltbdb.Controllers;
using ltbdb.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb
{
	public class AutomapperConfig
	{
		public static void RegisterAutomapper()
		{
			Mapper.Configuration.AllowNullDestinationValues = false;

			// Repository -> Domain
			Mapper.CreateMap<BookDTO, Book>()
				.ForMember(d => d.Created, map => map.MapFrom(s => s.Added))
				.ForMember(d => d.Category, map => map.MapFrom(s => new Category { Id = s.Category, Name = s.CategoryName }))
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToArray()));

			Mapper.CreateMap<CategoryDTO, Category>();

			Mapper.CreateMap<TagDTO, Tag>()
				.ForMember(d => d.References, map => map.MapFrom(s => s.Ref));

			//Domain -> View
			Mapper.CreateMap<Book, BookModel>()
				.ForMember(d => d.Category, map => map.MapFrom(s => s.Category.Id))
				.ForMember(d => d.CategoryName, map => map.MapFrom(s => s.Category.Name))
				.ForMember(d => d.Image, map => map.Ignore())
				.ForMember(d => d.Remove, map => map.Ignore());

			Mapper.CreateMap<Tag, TagModel>();

			Mapper.CreateMap<Category, CategoryModel>();

			//View -> Domain
			Mapper.CreateMap<BookModel, Book>()
				.ForMember(d => d.Category, map => map.MapFrom(s => new Category { Id = s.Category, Name = "" }))
				.ForSourceMember(s => s.Image, map => map.Ignore())
				.ForSourceMember(s => s.Remove, map => map.Ignore());

			Mapper.CreateMap<CategoryModel, Category>();

			Mapper.CreateMap<TagModel, Tag>();

			//Domain -> Repository
			Mapper.CreateMap<Book, BookDTO>()
				.ForMember(d => d.Stories, map => map.MapFrom(s => string.Join("|", s.Stories.Select(v => v.Trim().Replace("|", "_").Escape()).Where(v => !String.IsNullOrEmpty(v)))))
				.ForMember(d => d.Added, map => map.Ignore())
				.ForMember(d => d.Category, map => map.MapFrom(s => s.Category.Id))
				.ForMember(d => d.CategoryName, map => map.Ignore())
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Name.Trim().Escape()));

			Mapper.CreateMap<Category, CategoryDTO>()
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Name.Trim().Escape()));

			Mapper.CreateMap<Tag, TagDTO>()
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Name.Trim().Escape()))
				.ForMember(d => d.Ref, map => map.Ignore());

			//Domain -> WebService / REST
			Mapper.CreateMap<Book, ltbdb.Models.WebService.Book>()
				.ForMember(d => d.Category, map => map.MapFrom(s => new ltbdb.Models.WebService.Category { Id = s.Category.Id, Name = s.Category.Name }))
				.ForMember(d => d.Thumbnail, map => map.MapFrom(s => ImageStore.Exists(s.Filename, true) ? String.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, ImageStore.GetWebPath(s.Filename, ImageType.PreferThumbnail)) : null));

			Mapper.CreateMap<Category, ltbdb.Models.WebService.Category>()
				.ForMember(d => d.Id, map => map.MapFrom(s => s.Id))
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Name));

			Mapper.CreateMap<Tag, ltbdb.Models.WebService.Tag>()
				.ForMember(d => d.Id, map => map.MapFrom(s => s.Id))
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Name))
				.ForMember(d => d.References, map => map.MapFrom(s => s.References));

			Mapper.AssertConfigurationIsValid();
		}
	}
}