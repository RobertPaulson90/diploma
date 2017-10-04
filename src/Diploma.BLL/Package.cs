using System;
using AutoMapper;
using AutoMapper.Configuration;
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

            container.RegisterSingleton(() => GetMapper(container));

            container.RegisterSingleton<IUserService, UserService>();
            container.RegisterSingleton<ICryptoService, CryptoService>();
        }

        private static IMapper GetMapper(Container container)
        {
            var config = new MapperConfigurationExpression();
            config.ConstructServicesUsing(container.GetInstance);
            config.AddProfiles(typeof(Package).Assembly);

            var mapperConfiguration = new MapperConfiguration(config);
            mapperConfiguration.AssertConfigurationIsValid();

            return mapperConfiguration.CreateMapper(container.GetInstance);
        }
    }
}
