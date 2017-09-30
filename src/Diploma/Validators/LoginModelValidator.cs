using Diploma.Models;
using Diploma.Properties;
using FluentValidation;

namespace Diploma.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Username).NotEmpty().WithMessage(x => Resources.Authorization_Username_Can_Not_Be_Empty);

            RuleFor(x => x.Password).NotEmpty().WithMessage(x => Resources.Authorization_Password_Can_Not_Be_Empty);
        }
    }
}
