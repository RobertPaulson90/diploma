using System;
using Diploma.DAL.Contexts;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma.DAL
{
    public class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<Func<CompanyContext>>(() => container.GetInstance<CompanyContext>());
        }
    }
}
