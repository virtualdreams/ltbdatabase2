using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

namespace ltbdb.DomainServices.DTO
{
	public class StoryDTO
	{
		[SqlMapperAttributes(SqlMapperProperty.Required)]
		public int Id { get; set; }
		public int BookId { get; set; }
		public string Stories { get; set; }
	}
}