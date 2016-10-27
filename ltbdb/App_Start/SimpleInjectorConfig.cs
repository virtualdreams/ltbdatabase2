using MongoDB.Driver;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;

namespace ltbdb
{
	public class SimpleInjectorConfig
	{
		static public Container Container { get; private set; }

		static public void Register()
		{
			Container = new Container();

			// TODO - mongo server address
			Container.RegisterPerWebRequest<IMongoClient>(() => new MongoClient());

			Container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);
		}
	}
}