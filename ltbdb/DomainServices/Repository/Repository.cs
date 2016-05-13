using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ltbdb.DomainServices.Repository
{
	/// <summary>
	/// Basic repository
	/// </summary>
	public interface IRepository
	{
	}

	/// <summary>
	/// Basic typed repository
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepository<T> : IRepository where T : class, new()
	{
		T Add(T item);
		T Get(object id);
		IEnumerable<T> GetAll();
		T Update(T item);
		bool Delete(T item);
	}

	/// <summary>
	/// Basic abstract typed repository.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Repository<T> : IRepository<T> where T : class, new()
	{
		protected SqlConfig Config { get; private set; }
		protected SqlContext Context { get; private set; }

		protected Repository(SqlConfig config, SqlContext context)
		{
			this.Config = config;
			this.Context = context;
		}

		abstract public T Add(T item);
		abstract public T Get(object id);
		abstract public IEnumerable<T> GetAll();
		abstract public T Update(T item);
		abstract public bool Delete(T item);

		public int GetLastInsertId()
		{
			return this.Context.QueryForScalar<int>(this.Config.CreateQuery("getLastInsertId"));
		}
	}
}