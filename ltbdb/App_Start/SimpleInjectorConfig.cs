using ltbdb.Core;
using ltbdb.Core.Helpers;
using ltbdb.Core.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

			// TODO Register container
			Container.RegisterPerWebRequest<SqlConfig>(() => new SqlConfig(IOHelper.ConvertToFullPath(GlobalConfig.Get().Database)));
			Container.RegisterPerWebRequest<SqlContext>(() => Container.GetInstance<SqlConfig>().CreateContext());


			Container.Register<DemoService>();

			Container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);
		}
	}
}