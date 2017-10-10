using System;
using System.Threading;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Diploma.Core.Framework
{
    public sealed class BusyWatcher : PropertyChangedBase
    {
        private int _counter;

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            private set => Set(ref _isBusy, value);
        }

        [MustUseReturnValue]
        public IDisposable GetTicket()
        {
            return new BusyWatcherTicket(this);
        }

        private void AddWatch()
        {
            if (Interlocked.Increment(ref _counter) == 1)
            {
                IsBusy = true;
            }
        }

        private void RemoveWatch()
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

        private sealed class BusyWatcherTicket : IDisposable
        {
            private readonly BusyWatcher _parent;

            public BusyWatcherTicket(BusyWatcher parent)
            {
                _parent = parent;
                _parent.AddWatch();
            }

            public void Dispose()
            {
                _parent.RemoveWatch();
            }
        }
    }
}
