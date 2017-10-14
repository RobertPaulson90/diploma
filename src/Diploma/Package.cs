using System.Collections.Generic;
using System.Reflection;
using Diploma.Core.Framework.Validations;
using Diploma.Framework;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Services;
using Diploma.ViewModels;
using FluentValidation;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma
{
    public sealed class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register(typeof(IValidator<>), FluentValidatorsAssemblies(), Lifestyle.Singleton);
            container.Register(typeof(IValidationAdapter<>), typeof(FluentValidationAdapter<>));
            container.RegisterConditional(
                typeof(IValidationAdapter),
                c => typeof(FluentValidationAdapter<>).MakeGenericType(c.Consumer.ImplementationType),
                Lifestyle.Singleton,
                c => true);

            container.RegisterSingleton<IMessageService, MessageService>();

            container.RegisterSingleton<ShellViewModel>();
            container.RegisterSingleton<AuthenticationManagerViewModel>();
            container.Register<RegisterViewModel>();
            container.Register<LoginViewModel>();
            container.Register<DashboardViewModel>();
            container.Register<EditUserDataViewModel>();

            IEnumerable<Assembly> FluentValidatorsAssemblies()
            {
                yield return typeof(Package).Assembly;
            }
        }
    }
}
