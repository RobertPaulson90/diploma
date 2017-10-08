using Diploma.Properties;
using Diploma.ViewModels;
using FluentValidation;

namespace Diploma.Validators
{
    internal sealed class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(x => Resources.Validation_Authorization_Username_Can_Not_Be_Empty);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(x => Resources.Validation_Authorization_Password_Can_Not_Be_Empty);
        }
    }
}
