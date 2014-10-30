using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices.DTO
{
	public class Tag2BookDTO
	{
		[SqlMapperIn(Flag=SqlMapperFlags.Required)]
		public int Id { get; set; }
		public int TagId { get; set; }
		public int BookId { get; set; }
	}
}