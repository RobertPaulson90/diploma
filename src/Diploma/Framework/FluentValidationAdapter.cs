using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Core.Framework.Validations;
using FluentValidation;
using JetBrains.Annotations;

namespace Diploma.Framework
{
    internal sealed class FluentValidationAdapter<T> : IValidationAdapter<T>
    {
        [NotNull]
        private readonly IValidator<T> _validator;

        private T _subject;

        public FluentValidationAdapter([NotNull] IValidator<T> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }
        
        public async Task<Dictionary<string, IEnumerable<string>>> ValidateAllPropertiesAsync()
        {
            var validationResult = await _validator.ValidateAsync(_subject)
                .ConfigureAwait(false);

            var errors = validationResult.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(failure => failure.ErrorMessage));

            return errors;
        }

        public void Initialize(object subject)
        {
            _subject = (T)subject;
        }

        public async Task<IEnumerable<string>> ValidatePropertyAsync(string propertyName)
        {
            var validationResult = await _validator.ValidateAsync(_subject, propertyName)
                .ConfigureAwait(false);

            var errors = validationResult.Errors.Select(x => x.ErrorMessage);

            return errors;
        }
    }
}
