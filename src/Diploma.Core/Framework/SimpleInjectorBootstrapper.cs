using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using SimpleInjector;

namespace Diploma.Core.Framework
{
    public abstract class SimpleInjectorBootstrapper : BootstrapperBase
    {
        private Container _container;

        protected override sealed void BuildUp(object instance)
        {
            var registration = _container.GetRegistration(instance.GetType(), true);
            registration.Registration.InitializeInstance(instance);
        }

        protected override sealed void Configure()
        {
            _container = new Container();
            _container.RegisterSingleton<IWindowManager, WindowManager>();
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();
            _container.RegisterSingleton<IScreenFactory, ScreenFactory>();

            var assemblies = SelectPackageAssemblies()
                .ToList();

            _container.RegisterPackages(assemblies);

            _container.Verify();
        }

        protected override sealed IEnumerable<object> GetAllInstances(Type service)
        {
            IServiceProvider provider = _container;
            var collectionType = typeof(IEnumerable<>).MakeGenericType(service);
            var services = (IEnumerable<object>)provider.GetService(collectionType);
            return services ?? Enumerable.Empty<object>();
        }

        protected override sealed object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service);
        }
        
        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Dispose();
        }
        
        protected abstract IEnumerable<Assembly> SelectPackageAssemblies();
    }
}
