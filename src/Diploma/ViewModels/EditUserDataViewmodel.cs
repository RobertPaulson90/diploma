using System;
using Diploma.BLL.Queries.Responses;
using Diploma.Core.Framework.Validations;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    public sealed class EditUserDataViewModel : ValidatableScreen
    {
        private DateTime? _birthDate;

        private string _firstName;

        private GenderType _gender;

        private string _lastName;

        private string _middleName;

        public EditUserDataViewModel([NotNull] IValidationAdapter<EditUserDataViewModel> validationAdapter)
            : base(validationAdapter)
        {
            Validate();
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set => Set(ref _birthDate, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        public GenderType Gender
        {
            get => _gender;
            set => Set(ref _gender, value);
        }

        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }

        public string MiddleName
        {
            get => _middleName;
            set => Set(ref _middleName, value);
        }
    }
}
