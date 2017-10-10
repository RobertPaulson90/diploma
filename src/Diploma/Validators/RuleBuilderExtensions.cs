using System;
using Diploma.Validators.PropertyValidators;
using FluentValidation;

namespace Diploma.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, DateTime?> BirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new BirthDateValidator());
        }
    }
}
