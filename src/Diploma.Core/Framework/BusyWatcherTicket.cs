using System;

namespace Diploma.Core.Framework
{
    internal sealed class BusyWatcherTicket : IDisposable
    {
        private readonly BusyWatcher _parent;

        private bool _disposed;

        public BusyWatcherTicket(BusyWatcher parent)
        {
            _parent = parent;
            _parent.AddWatch();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _parent.RemoveWatch();
            _disposed = true;
        }
    }
}