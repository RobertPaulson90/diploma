using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using FluentValidation;
using FluentValidation.Results;

namespace Diploma.Framework.Validations
{
    public abstract class ValidatableScreen<TProperty, TValidator> : Screen, INotifyDataErrorInfo
        where TProperty : Screen
        where TValidator : IValidator<TProperty>
    {
        private readonly TProperty _target;

        private readonly TValidator _validator;

        private ValidationResult _validationResult;

        private bool _hasErrors;

        protected ValidatableScreen(TValidator validator)
        {
            _target = this as TProperty;
            if (_target == null)
            {
                throw new ArgumentException("Invalid type of screen passed.");
            }

            _validator = validator;
            _validationResult = _validator.Validate(_target);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get
            {
                return _hasErrors;
            }

            private set
            {
                Set(ref _hasErrors, value);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationResult.Errors.Where(x => x.PropertyName == propertyName).Select(x => x.ErrorMessage);
        }

        public virtual bool Validate()
        {
            _validationResult = _validator.Validate(_target);

            foreach (var error in _validationResult.Errors)
            {
                OnUIThread(() => RaiseErrorsChanged(error.PropertyName));
            }

            HasErrors = _validationResult.Errors.Count > 0;
            return HasErrors;
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
