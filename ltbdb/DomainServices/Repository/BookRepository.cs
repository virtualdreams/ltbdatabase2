using ltbdb.DomainServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlDataMapper;

namespace ltbdb.DomainServices.Repository
{
	public class BookRepository: Repository<BookDTO>
	{
		public BookRepository(SqlConfig config, SqlContext context)
			: base(config, context)
		{
		}

		public override BookDTO Add(BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override BookDTO Get(object id)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<BookDTO> GetAll()
		{
			BookDTO[] books = new BookDTO[] { 
				new BookDTO { Id = 211, Category = 1, CategoryName = "LTB", Name = "Demo1", Number = 231, Added = DateTime.Now},
				new BookDTO { Id = 212, Category = 1, CategoryName = "LTB", Name = "Demo2", Number = 232, Added = DateTime.Now},
				new BookDTO { Id = 213, Category = 1, CategoryName = "LTB", Name = "Demo3", Number = 233, Added = DateTime.Now},
				new BookDTO { Id = 214, Category = 1, CategoryName = "LTB", Name = "Demo4", Number = 234, Added = DateTime.Now},
				new BookDTO { Id = 215, Category = 1, CategoryName = "LTB", Name = "Demo5", Number = 235, Added = DateTime.Now}
			};

			return books;
		}

		public override void Update(BookDTO item)
		{
			throw new NotImplementedException();
		}

		public override void Delete(BookDTO item)
		{
			throw new NotImplementedException();
		}
	}
}