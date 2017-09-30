using System;
using Diploma.DAL.Entities;
using Diploma.Framework.Validations;
using FluentValidation;

namespace Diploma.Models
{
    public class RegisterModel : ValidatablePropertyChanged<RegisterModel, IValidator<RegisterModel>>
    {
        private DateTime? _birthDate;

        private string _confirmPassword;

        private string _firstName;

        private GenderType _gender;

        private string _lastName;

        private string _middleName;

        private string _password;

        private string _username;

        public RegisterModel(IValidator<RegisterModel> validator)
            : base(validator)
        {
        }

        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }

            set
            {
                if (Set(ref _birthDate, value))
                {
                    Validate();
                }
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
                if (Set(ref _confirmPassword, value))
                {
                    Validate();
                }
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
                if (Set(ref _firstName, value))
                {
                    Validate();
                }
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
                if (Set(ref _gender, value))
                {
                    Validate();
                }
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
                if (Set(ref _lastName, value))
                {
                    Validate();
                }
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
                if (Set(ref _middleName, value))
                {
                    Validate();
                }
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
                if (Set(ref _password, value))
                {
                    Validate();
                }
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                if (Set(ref _username, value))
                {
                    Validate();
                }
            }
        }
    }
}
