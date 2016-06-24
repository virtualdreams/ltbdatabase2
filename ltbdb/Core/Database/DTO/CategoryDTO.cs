using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

namespace ltbdb.Core.Database.DTO
{
	public class CategoryDTO
	{
		[Required]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}