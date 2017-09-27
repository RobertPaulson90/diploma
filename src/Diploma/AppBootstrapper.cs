using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Diploma.DAL;
using Diploma.Entities;
using Diploma.ViewModels;

namespace Diploma
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            using (var context = new CompanyContext())
            {
                if (!context.Users.Any())
                {
                    User[] users =
                    {
                        new Programmer
                        {
                            LastName = "Привалов",
                            FirstName = "Максим",
                            MiddleName = "Юрьевич",
                            Username = "privalov_mu",
                            Salary = 11000,
                            Gender = GenderType.Male,
                            Password = "test"
                        },
                        new Programmer
                        {
                            LastName = "Калинин",
                            FirstName = "Василий",
                            MiddleName = "Александрович",
                            Username = "kalinin_va",
                            Salary = 9000,
                            Gender = GenderType.Male,
                            Password = "test"
                        },
                        new Manager
                        {
                            LastName = "Кравченко",
                            FirstName = "Людмила",
                            MiddleName = "Николаевна",
                            Username = "kravchenko_ln",
                            Salary = 7500,
                            Gender = GenderType.Female,
                            Password = "test"
                        },
                        new Customer
                        {
                            LastName = "Шевченко",
                            FirstName = "Татьяна",
                            MiddleName = "Леонидовна",
                            Username = "shevchenko_tl",
                            Gender = GenderType.Female,
                            Password = "test"
                        }
                    };

                    context.Users.AddRange(users);

                    context.SaveChanges();
                }
            }

            DisplayRootViewFor<ShellViewModel>();
        }
    }
}