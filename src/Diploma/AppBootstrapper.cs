using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Diploma.ViewModels;
using SimpleInjector;

namespace Diploma
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        private readonly Container _container = new Container();

        public AppBootstrapper()
        {
            Initialize();
            ChangeLocalization();
        }

        protected override void BuildUp(object instance)
        {
            var registration = _container.GetRegistration(instance.GetType(), true);
            registration.Registration.InitializeInstance(instance);
        }

        protected override void Configure()
        {
            RegisterDefaultServices(_container);
            _container.RegisterPackages(SelectAssemblies());
            _container.Verify();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            IServiceProvider provider = _container;
            var collectionType = typeof(IEnumerable<>).MakeGenericType(service);
            var services = (IEnumerable<object>)provider.GetService(collectionType);
            return services ?? Enumerable.Empty<object>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Dispose();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic);
        }

        private static void ChangeLocalization()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        private static void RegisterDefaultServices(Container container)
        {
            container.RegisterSingleton<IWindowManager, WindowManager>();
            container.RegisterSingleton<IEventAggregator, EventAggregator>();
        }
    }
}
