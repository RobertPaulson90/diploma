using System;
using Diploma.Core.Properties;
using FluentValidation.Validators;

namespace Diploma.Core.Framework.Validations.PropertyValidators
{
    public class BirthDateValidator : PropertyValidator
    {
        private const int DefaultMaximumAge = 131;

        private const int DefaultMinimumAge = 2;

        private readonly int _maximumAge;

        private readonly int _minimumAge;

        public BirthDateValidator()
            : this(DefaultMinimumAge, DefaultMaximumAge)
        {
        }

        public BirthDateValidator(int minimumAge, int maximumAge)
            : base(nameof(Resources.BirthDateValidator_Default_Validation_Message), typeof(Resources))
        {
            if (maximumAge < minimumAge)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumAge), Resources.Exception_BirthDate_Wrong_Maximum_Age);
            }

            _minimumAge = minimumAge;
            _maximumAge = maximumAge;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var birthDate = (DateTime?)context.PropertyValue;

            if (birthDate == null)
            {
                return true;
            }

            var minimumBirthDate = DateTime.Today.AddYears(-_maximumAge);
            var maximumBirthDate = DateTime.Today.AddYears(-_minimumAge);

            if (birthDate >= minimumBirthDate && birthDate < maximumBirthDate)
            {
                return true;
            }

            context.MessageFormatter.AppendArgument("From", minimumBirthDate)
                .AppendArgument("To", maximumBirthDate)
                .AppendArgument("Value", context.PropertyValue);

            return false;
        }
    }
}
