using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

namespace ltbdb.DomainServices.DTO
{
	public class BookDTO
	{
		[SqlMapperIn(Flag=SqlMapperFlags.Required)]
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public int Category { get; set; }
		public string CategoryName { get; set; }
		public DateTime Added { get; set; }
	}
}