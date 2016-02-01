using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

namespace ltbdb.DomainServices.DTO
{
	public class TagDTO
	{
		[Required]
		public int Id { get; set; }
		public string Name { get; set; }
		public long Ref { get; set; }
	}
}