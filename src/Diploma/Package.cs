using System;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Services;
using Diploma.Validators;
using Diploma.ViewModels;
using FluentValidation;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Diploma
{
    public class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<IMessageService, MessageService>();

            container.Register<ShellViewModel>();

            container.Register<AuthenticationManagerViewModel>();

            container.Register<RegisterViewModel>();
            container.RegisterSingleton<Func<RegisterViewModel>>(() => container.GetInstance<RegisterViewModel>());
            container.RegisterSingleton<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
            
            container.Register<LoginViewModel>();
            container.RegisterSingleton<Func<LoginViewModel>>(() => container.GetInstance<LoginViewModel>());
            container.RegisterSingleton<IValidator<LoginViewModel>, LoginViewModelValidator>();

            container.Register<DashboardViewModel>();

            container.Register<EditUserDataViewModel>();
            container.RegisterSingleton<IValidator<EditUserDataViewModel>, EditUserDataViewModelValidator>();
        }
    }
}
