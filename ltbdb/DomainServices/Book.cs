using AutoMapper;
using ltbdb.DomainServices.DTO;
using ltbdb.DomainServices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class Book: DatabaseContext
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }
		public DateTime Created { get; set; }

		/// <summary>
		/// Get all tags related to this book.
		/// </summary>
		/// <returns></returns>
		public Tag[] GetTags()
		{
			TagRepository tagRepo = new TagRepository(this.Config, this.Context);

			var tags = tagRepo.GetByBook(this.Id);

			var mapper = Mapper.CreateMap<TagDTO, Tag>();

			var result = Mapper.Map<IEnumerable<TagDTO>, Tag[]>(tags);

			return result;
		}
	}
}