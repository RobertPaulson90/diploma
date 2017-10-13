using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;

namespace Diploma.Core.Framework
{
    public class BusyWatcher : INotifyPropertyChanged
    {
        private int _counter;

        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        [MustUseReturnValue]
        public virtual IDisposable GetTicket()
        {
            return new BusyWatcherTicket(this);
        }

        internal virtual void AddWatch()
        {
            if (Interlocked.Increment(ref _counter) == 1)
            {
                IsBusy = true;
            }
        }

        internal virtual void RemoveWatch()
        {
            if (_counter == 0)
            {
                throw new InvalidOperationException("NoMatchingAddWatch"); // TODO: Add to resources
            }

            if (Interlocked.Decrement(ref _counter) == 0)
            {
                IsBusy = false;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
