using System;
using Diploma.BLL.DTO.Enums;
using Diploma.Framework.Validations;
using FluentValidation;

namespace Diploma.ViewModels
{
    public class EditUserDataViewModel : ValidatableScreen<EditUserDataViewModel, IValidator<EditUserDataViewModel>>
    {
        private DateTime? _birthDate;

        private string _firstName;

        private GenderType _gender;

        private string _lastName;

        private string _middleName;

        public EditUserDataViewModel(IValidator<EditUserDataViewModel> validator)
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
    }
}
