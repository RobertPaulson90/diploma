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
        private readonly Dictionary<string, string[]> _propertyErrors;

        private readonly SemaphoreSlim _propertyErrorsLock;

        private readonly IValidationAdapter _validator;

        private ValidatableScreen()
        {
            _propertyErrorsLock = new SemaphoreSlim(1, 1);
            _propertyErrors = new Dictionary<string, string[]>();
            AutoValidate = true;
        }

        protected ValidatableScreen([NotNull] IValidationAdapter validationAdapter)
            : this()
        {
            _validator = validationAdapter ?? throw new ArgumentNullException(nameof(validationAdapter));
            _validator.Initialize(this);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _propertyErrors.Values.Any(x => x != null && x.Length > 0);

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

        public override async void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            base.NotifyOfPropertyChange(propertyName);

            if (_validator != null && AutoValidate && propertyName != nameof(HasErrors))
            {
                var _ = await ValidatePropertyAsync(propertyName)
                    .ConfigureAwait(false);
            }
        }

        protected virtual void OnValidationStateChanged(IEnumerable<string> changedProperties)
        {
            foreach (var property in changedProperties)
            {
                RaiseErrorsChanged(property);
            }

            NotifyOfPropertyChange(nameof(HasErrors));
        }

        protected virtual void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                OnUIThread(() => handler(this, new DataErrorsChangedEventArgs(propertyName)));
            }
        }
        
        protected bool Validate()
        {
            try
            {
                return ValidateAsync()
                    .Result;
            }
            catch (AggregateException e) when (e.InnerException != null)
            {
                throw e.InnerException;
            }
        }

        protected virtual async Task<bool> ValidateAsync()
        {
            if (_validator == null)
            {
                throw new InvalidOperationException("Can't run validation if a validator hasn't been set");
            }

            var results = await _validator.ValidateAllPropertiesAsync()
                              .ConfigureAwait(false) ?? new Dictionary<string, IEnumerable<string>>();

            await _propertyErrorsLock.WaitAsync()
                .ConfigureAwait(false);
            try
            {
                var changedProperties = new List<string>();

                foreach (var kvp in results)
                {
                    var newErrors = kvp.Value?.ToArray();
                    if (!_propertyErrors.ContainsKey(kvp.Key))
                    {
                        _propertyErrors[kvp.Key] = newErrors;
                    }
                    else if (ErrorsEqual(_propertyErrors[kvp.Key], newErrors))
                    {
                        continue;
                    }
                    else
                    {
                        _propertyErrors[kvp.Key] = newErrors;
                    }

                    changedProperties.Add(kvp.Key);
                }

                foreach (var removedKey in _propertyErrors.Keys.Except(results.Keys)
                    .ToArray())
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

        protected virtual bool ValidateProperty<TProperty>(Expression<Func<TProperty>> property)
        {
            return ValidateProperty(property.GetMemberInfo().Name);
        }

        protected bool ValidateProperty([CallerMemberName] string propertyName = null)
        {
            try
            {
                return ValidatePropertyAsync(propertyName)
                    .Result;
            }
            catch (AggregateException e) when (e.InnerException != null)
            {
                throw e.InnerException;
            }
        }

        protected virtual Task<bool> ValidatePropertyAsync<TProperty>(Expression<Func<TProperty>> property)
        {
            return ValidatePropertyAsync(property.GetMemberInfo().Name);
        }

        protected virtual async Task<bool> ValidatePropertyAsync([CallerMemberName] string propertyName = null)
        {
            if (_validator == null)
            {
                throw new InvalidOperationException("Can't run validation if a validator hasn't been set");
            }

            if (propertyName == null)
            {
                propertyName = string.Empty;
            }

            var newErrorsRaw = await _validator.ValidatePropertyAsync(propertyName)
                .ConfigureAwait(false);

            var newErrors = newErrorsRaw?.ToArray();
            var propertyErrorsChanged = false;

            await _propertyErrorsLock.WaitAsync()
                .ConfigureAwait(false);
            try
            {
                if (!_propertyErrors.ContainsKey(propertyName))
                {
                    _propertyErrors.Add(propertyName, null);
                }

                if (!ErrorsEqual(_propertyErrors[propertyName], newErrors))
                {
                    _propertyErrors[propertyName] = newErrors;
                    propertyErrorsChanged = true;
                }
            }
            finally
            {
                _propertyErrorsLock.Release();
            }

            if (propertyErrorsChanged)
            {
                OnValidationStateChanged(new[] { propertyName });
            }

            return newErrors == null || newErrors.Length == 0;
        }

        private bool ErrorsEqual(string[] e1, string[] e2)
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
