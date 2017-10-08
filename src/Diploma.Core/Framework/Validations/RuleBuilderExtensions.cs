using System;
using Diploma.Core.Framework.Validations.PropertyValidators;
using FluentValidation;

namespace Diploma.Core.Framework.Validations
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, DateTime?> BirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new BirthDateValidator());
        }
    }
}
