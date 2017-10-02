using Diploma.Common.Properties;
using Diploma.ViewModels;
using FluentValidation;

namespace Diploma.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Username).NotEmpty().WithMessage(x => Resources.Authorization_Username_Can_Not_Be_Empty);

            RuleFor(x => x.Password).NotEmpty().WithMessage(x => Resources.Authorization_Password_Can_Not_Be_Empty);
        }
    }
}
