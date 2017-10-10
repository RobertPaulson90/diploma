using System;
using Diploma.Core.Properties;
using FluentValidation.Validators;

namespace Diploma.Validators.PropertyValidators
{
    public sealed class BirthDateValidator : PropertyValidator
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
            : base(nameof(Resources.Validation_BirthDateValidator_Default_Message), typeof(Resources))
        {
            if (minimumAge < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumAge), string.Format(Resources.Exception_BirthDateValidator_Minimum_Age_Non_Positive, minimumAge));
            }

            if (maximumAge < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumAge), string.Format(Resources.Exception_BirthDateValidator_Maximum_Age_Non_Positive, maximumAge));
            }

            if (maximumAge < minimumAge)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumAge), string.Format(Resources.Exception_BirthDateValidator_Maximum_Age_Less_Than_Minimum_Age, maximumAge, minimumAge));
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

            context.MessageFormatter
                .AppendArgument("Min", minimumBirthDate)
                .AppendArgument("Max", maximumBirthDate);

            return false;
        }
    }
}
