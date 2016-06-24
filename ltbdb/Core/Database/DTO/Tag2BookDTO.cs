using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Database.DTO
{
	public class Tag2BookDTO
	{
		[Required]
		public int Id { get; set; }
		public int TagId { get; set; }
		public int BookId { get; set; }
	}
}