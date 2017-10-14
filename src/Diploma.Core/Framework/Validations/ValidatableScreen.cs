using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Diploma.Core.Framework.Validations
{
    public abstract class ValidatableScreen : Screen, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, string[]> _propertyErrors = new Dictionary<string, string[]>();

        private readonly SemaphoreSlim _propertyErrorsLock = new SemaphoreSlim(1, 1);

        [NotNull]
        private readonly IValidationAdapter _validator;

        protected ValidatableScreen([NotNull] IValidationAdapter validationAdapter)
        {
            _validator = validationAdapter ?? throw new ArgumentNullException(nameof(validationAdapter));
            _validator.Initialize(this);
            AutoValidate = true;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _propertyErrors.Values.Any(x => x != null && x.Length > 0);

        [UsedImplicitly]
        protected bool AutoValidate { get; set; }

        public IEnumerable GetErrors(string propertyName)
        {
            _propertyErrorsLock.Wait();
            try
            {
                if (propertyName == null)
                {
                    propertyName = string.Empty;
                }

                _propertyErrors.TryGetValue(propertyName, out var errors);

                return errors;
            }
            finally
            {
                _propertyErrorsLock.Release();
            }
        }

        public new virtual async void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            base.NotifyOfPropertyChange(propertyName);

            if (AutoValidate)
            {
                var _ = await ValidatePropertyAsync(propertyName)
                    .ConfigureAwait(false);
            }
        }

        public new virtual bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            oldValue = newValue;
            NotifyOfPropertyChange(propertyName ?? string.Empty);
            return true;
        }

        protected virtual void NotifyOfErrorsChanged(string propertyName)
        {
            if (ErrorsChanged == null)
            {
                return;
            }

            OnUIThread(() => OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName)));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var errorsChanged = ErrorsChanged;

            errorsChanged?.Invoke(this, e);
        }

        protected virtual void OnValidationStateChanged(IEnumerable<string> propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                NotifyOfErrorsChanged(propertyName);
            }

            base.NotifyOfPropertyChange(nameof(HasErrors));
        }

        protected virtual void OnValidationStateChanged(string propertyName)
        {
            NotifyOfErrorsChanged(propertyName);

            base.NotifyOfPropertyChange(nameof(HasErrors));
        }

        protected virtual bool Validate()
        {
            return ValidateAsync()
                .GetAwaiter()
                .GetResult();
        }

        protected virtual async Task<bool> ValidateAsync()
        {
            var results = await _validator.ValidateAllPropertiesAsync()
                .ConfigureAwait(false);

            await _propertyErrorsLock.WaitAsync()
                .ConfigureAwait(false);
            try
            {
                var changedProperties = new List<string>();

                foreach (var kvp in results)
                {
                    var propertyName = kvp.Key;
                    var propertyErrors = kvp.Value;
                    if (_propertyErrors.ContainsKey(propertyName) && ErrorsEqual(_propertyErrors[propertyName], propertyErrors))
                    {
                        continue;
                    }

                    _propertyErrors[propertyName] = propertyErrors;
                    changedProperties.Add(propertyName);
                }

                var keys = _propertyErrors.Keys.Except(results.Keys).ToArray();
                foreach (var removedKey in keys)
                {
                    _propertyErrors[removedKey] = null;
                    changedProperties.Add(removedKey);
                }

                if (changedProperties.Count > 0)
                {
                    OnValidationStateChanged(changedProperties);
                }

                return !HasErrors;
            }
            finally
            {
                _propertyErrorsLock.Release();
            }
        }

        protected virtual bool ValidateProperty<TProperty>([NotNull] Expression<Func<TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return ValidateProperty(property.GetMemberInfo().Name);
        }

        protected virtual bool ValidateProperty([CallerMemberName] string propertyName = null)
        {
            return ValidatePropertyAsync(propertyName)
                .GetAwaiter()
                .GetResult();
        }
        
        protected virtual Task<bool> ValidatePropertyAsync<TProperty>([NotNull] Expression<Func<TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return ValidatePropertyAsync(property.GetMemberInfo().Name);
        }

        protected virtual async Task<bool> ValidatePropertyAsync([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                propertyName = string.Empty;
            }

            var propertyErrors = await _validator.ValidatePropertyAsync(propertyName)
                .ConfigureAwait(false);
            
            var propertyErrorsChanged = false;

            await _propertyErrorsLock.WaitAsync()
                .ConfigureAwait(false);

            try
            {
                if (!_propertyErrors.ContainsKey(propertyName))
                {
                    _propertyErrors.Add(propertyName, null);
                }

                if (!ErrorsEqual(_propertyErrors[propertyName], propertyErrors))
                {
                    _propertyErrors[propertyName] = propertyErrors;
                    propertyErrorsChanged = true;
                }
            }
            finally
            {
                _propertyErrorsLock.Release();
            }

            if (propertyErrorsChanged)
            {
                OnValidationStateChanged(propertyName);
            }

            return propertyErrors == null || propertyErrors.Length == 0;
        }

        private static bool ErrorsEqual(string[] e1, string[] e2)
        {
            if (e1 == null && e2 == null)
            {
                return true;
            }

            if (e1 == null || e2 == null)
            {
                return false;
            }

            return e1.SequenceEqual(e2);
        }
    }
}
