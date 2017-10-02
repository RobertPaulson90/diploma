using System;
using FluentValidation;

namespace Diploma.Validators.Properties
{
    public static class Validators
    {
        public static IRuleBuilderOptions<T, DateTime?> BirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new BirthDateValidator());
        }
    }
}