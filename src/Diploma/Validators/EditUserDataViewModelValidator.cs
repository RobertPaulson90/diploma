using Diploma.Framework.Validations;
using Diploma.Properties;
using Diploma.ViewModels;
using FluentValidation;

namespace Diploma.Validators
{
    internal sealed class EditUserDataViewModelValidator : AbstractValidator<EditUserDataViewModel>
    {
        public EditUserDataViewModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(x => Resources.Validation_Editing_FirstName_Can_Not_Be_Empty);

            RuleFor(x => x.LastName).NotEmpty().WithMessage(x => Resources.Validation_Editing_LastName_Can_Not_Be_Empty);

            RuleFor(x => x.BirthDate).BirthDate().WithMessage(x => Resources.Validation_Editing_BirthDate_Must_Be_Be_Valid_Age);
        }
    }
}
