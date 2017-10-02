using System;
using Diploma.Framework.Validations.PropertyValidators;
using FluentValidation;

namespace Diploma.Framework.Validations
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, DateTime?> BirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new BirthDateValidator());
        }
    }
}