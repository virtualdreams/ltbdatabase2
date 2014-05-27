using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Windsor.Installer
{
	public class CastleInstaller: IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Classes.FromThisAssembly().Where(type => type.Name.EndsWith("Castle")).WithServiceDefaultInterfaces().Configure(c => c.LifestyleTransient()));
		}
	}
}