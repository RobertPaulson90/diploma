using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using FluentValidation;
using FluentValidation.Results;

namespace Diploma.Framework.Validations
{
    public abstract class ValidatablePropertyChanged<TProperty, TValidator> : PropertyChangedBase, IDataErrorInfo, INotifyDataErrorInfo
        where TProperty : PropertyChangedBase
        where TValidator : IValidator<TProperty>
    {
        private readonly TProperty _target;

        private readonly TValidator _validator;

        private ValidationResult _validationResult;

        protected ValidatablePropertyChanged(TValidator validator)
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

        public string Error
        {
            get
            {
                var strings = _validationResult.Errors.Select(x => x.ErrorMessage).ToArray();
                return string.Join(Environment.NewLine, strings);
            }
        }

        public bool HasErrors => _validationResult.Errors.Count > 0;

        public string this[string propertyName]
        {
            get
            {
                var errors = _validationResult.Errors.Where(x => x.PropertyName == propertyName).Select(x => x.ErrorMessage);
                return string.Join(Environment.NewLine, errors);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationResult.Errors.Where(x => x.PropertyName == propertyName).Select(x => x.ErrorMessage);
        }

        protected virtual bool Validate()
        {
            _validationResult = _validator.Validate(_target);

            foreach (var error in _validationResult.Errors)
            {
                OnUIThread(() => RaiseErrorsChanged(error.PropertyName));
            }

            return HasErrors;
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
