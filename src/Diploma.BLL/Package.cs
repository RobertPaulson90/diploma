using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;
using Diploma.BLL.Services;
using Diploma.BLL.Services.Interfaces;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma.BLL
{
    public sealed class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton(() => GetMapper(container));

            container.RegisterSingleton<IUserService, UserService>();
            container.RegisterSingleton<IPasswordHasher, PasswordHasher>();

            var assemblies = GetAssemblies()
                .ToList();

            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.Register(typeof(IRequestHandler<>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<>), assemblies);
            container.Register(typeof(ICancellableAsyncRequestHandler<>), assemblies);
            container.Register(typeof(ICancellableAsyncRequestHandler<,>), assemblies);
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);
            
            container.RegisterCollection(
                typeof(IPipelineBehavior<,>),
                new[] { typeof(RequestPreProcessorBehavior<,>), typeof(RequestPostProcessorBehavior<,>) });

            container.RegisterCollection(typeof(IRequestPreProcessor<>));
            container.RegisterCollection(typeof(IRequestPostProcessor<,>));

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).Assembly;
            yield return typeof(Package).Assembly;
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
