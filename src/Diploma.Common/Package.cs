using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma.Common
{
    public class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton(() => GetMapper(container));
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
