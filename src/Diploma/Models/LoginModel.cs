using Diploma.Framework.Validations;
using FluentValidation;

namespace Diploma.Models
{
    public class LoginModel : ValidatablePropertyChanged<LoginModel, IValidator<LoginModel>>
    {
        private string _password;

        private string _username;

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

        public LoginModel(IValidator<LoginModel> validator)
            : base(validator)
        {
        }
    }
}