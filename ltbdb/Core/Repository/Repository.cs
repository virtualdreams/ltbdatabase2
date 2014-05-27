using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ltbdb.Core
{
	public interface IRepository
	{
	}

	public interface IRepository<T> : IRepository where T : class, new()
	{
		T Add(T item);
		T Get(object id);
		IQueryable<T> GetAll();
		void Update(T item);
		void Delete(T item);
	}

	public abstract class Repository<T> : IRepository<T> where T : class, new()
	{
		public SqlConfig Config { get; private set; }
		public SqlContext Context { get; private set; }

		protected Repository(SqlConfig config, SqlContext context)
		{
			this.Config = config;
			this.Context = context;
		}

		abstract public T Add(T item);
		abstract public T Get(object id);
		abstract public IQueryable<T> GetAll();
		abstract public void Update(T item);
		abstract public void Delete(T item);
	}
}