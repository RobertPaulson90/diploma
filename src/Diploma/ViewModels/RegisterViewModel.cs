using System;
using Caliburn.Micro;
using Diploma.DAL;
using Diploma.Entities;

namespace Diploma.ViewModels
{
    public sealed class RegisterViewModel : Screen
    {
        private string _username;

        private string _lastName;

        private string _firstName;

        private string _middleName;

        private string _password;

        private string _confirmPassword;

        private DateTime? _birthDate;

        private UserRoleType _userRole;

        private GenderType _gender;

        public RegisterViewModel()
        {
            DisplayName = "Registration";
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                Set(ref _username, value);
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                Set(ref _lastName, value);
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                Set(ref _firstName, value);
            }
        }

        public string MiddleName
        {
            get
            {
                return _middleName;
            }

            set
            {
                Set(ref _middleName, value);
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                Set(ref _password, value);
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }

            set
            {
                Set(ref _confirmPassword, value);
            }
        }

        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }

            set
            {
                Set(ref _birthDate, value);
            }
        }

        public UserRoleType UserRole
        {
            get
            {
                return _userRole;
            }

            set
            {
                Set(ref _userRole, value);
            }
        }

        public GenderType Gender
        {
            get
            {
                return _gender;
            }

            set
            {
                Set(ref _gender, value);
            }
        }

        public void Register()
        {
            User user;
            switch (UserRole)
            {
                case UserRoleType.Customer:
                    user = new Customer();
                    break;
                case UserRoleType.Programmer:
                    user = new Programmer();
                    break;
                case UserRoleType.Manager:
                    user = new Manager();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            user.FirstName = FirstName;
            user.LastName = LastName;
            user.BirthDate = BirthDate;
            user.Gender = Gender;
            user.Username = Username;
            user.Password = Password;
            user.MiddleName = MiddleName;

            using (var context = new CompanyContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void Cancel()
        {
            ((ShellViewModel)Parent).ActiveItem = new LoginViewModel();
        }
    }
}
