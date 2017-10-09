using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using Diploma.Core.Framework;
using Diploma.ViewModels;

namespace Diploma
{
    public sealed class AppBootstrapper : SimpleInjectorBootstrapper
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
        
        protected override void PreInitialize()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        protected override IEnumerable<Assembly> SelectPackageAssemblies()
        {
            yield return typeof(DAL.Package).Assembly;
            yield return typeof(BLL.Package).Assembly;
            yield return typeof(Package).Assembly;
        }
    }
}
