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
            var fluentValidatorsAssemblies = GetAssemblies();
            container.Register(typeof(IValidator<>), fluentValidatorsAssemblies);
            container.Register(typeof(IValidationAdapter<>), typeof(FluentValidationAdapter<>));

            container.RegisterSingleton<IMessageService, MessageService>();

            container.Register<ShellViewModel>();
            container.Register<AuthenticationManagerViewModel>();
            container.Register<RegisterViewModel>();
            container.Register<LoginViewModel>();
            container.Register<DashboardViewModel>();
            container.Register<EditUserDataViewModel>();
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(Package).Assembly;
        }
    }
}
