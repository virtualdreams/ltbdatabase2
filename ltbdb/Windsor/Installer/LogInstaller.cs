using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Windsor.Installer
{
	public class LogInstaller: IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.AddFacility<LoggingFacility>(f => f.UseLog4Net(IOHelper.ConvertToFullPath("./App_Data/log4net.xml")));
		}
	}
}