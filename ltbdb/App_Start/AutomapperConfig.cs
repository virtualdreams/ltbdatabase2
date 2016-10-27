using AutoMapper;
using ltbdb.Models;
using System;
using System.Linq;

using ltbdb.Core.Models;

namespace ltbdb
{
	public class AutomapperConfig
	{
		public static void RegisterAutomapper()
		{
			//Mapper.Configuration.AllowNullDestinationValues = false;

			Mapper.CreateMap<Book, BookModel>()
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Title)); // TODO rename from name to title

			Mapper.CreateMap<Book, BookWriteModel>()
				.ForMember(d => d.Name, map => map.MapFrom(s => s.Title)) // TODO rename from name to title
				.ForMember(d => d.Tags, map => map.MapFrom(s => String.Join("; ", s.Tags)))
				.ForMember(d => d.Image, map => map.Ignore())
				.ForMember(d => d.Remove, map => map.Ignore());

			Mapper.CreateMap<BookWriteModel, Book>()
				.ForMember(d => d.Title, map => map.MapFrom(s => s.Name.Trim()))
				.ForMember(d => d.Category, map => map.MapFrom(s => s.Category.Trim()))
				.ForMember(s => s.Created, map => map.Ignore())
				.ForMember(d => d.Filename, map => map.Ignore())
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Select(x => x.Trim()).Where(w => !String.IsNullOrEmpty(w))))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(w => !String.IsNullOrEmpty(w))))
				.ForSourceMember(s => s.Image, map => map.Ignore())
				.ForSourceMember(s => s.Remove, map => map.Ignore());
				

			Mapper.AssertConfigurationIsValid();
		}
	}
}