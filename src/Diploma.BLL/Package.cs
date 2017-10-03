using System;
using Diploma.BLL.Interfaces.Services;
using Diploma.BLL.Services;
using Diploma.DAL.Contexts;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma.BLL
{
    public class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<Func<CompanyContext>>(() => container.GetInstance<CompanyContext>());

            container.RegisterSingleton<IUserService, UserService>();
            container.RegisterSingleton<ICryptoService, CryptoService>();
        }
    }
}
