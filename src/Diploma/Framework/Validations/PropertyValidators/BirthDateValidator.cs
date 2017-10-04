﻿using System;
using Diploma.Properties;
using FluentValidation.Validators;

namespace Diploma.Framework.Validations.PropertyValidators
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
            : base("The '{PropertyName}' is not a valid date of birth. It must be between {From} and {To}. You entered {Value}.")
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

            context.MessageFormatter.AppendArgument("From", minimumBirthDate).AppendArgument("To", maximumBirthDate)
                .AppendArgument("Value", context.PropertyValue);

            return false;
        }
    }
}