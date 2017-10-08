using System;
using Diploma.DAL.Contexts;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma.DAL
{
    public sealed class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<Func<CompanyContext>>(() => container.GetInstance<CompanyContext>());
        }
    }
}
