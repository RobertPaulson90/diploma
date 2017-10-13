using System;

namespace Diploma.Core.Framework
{
    internal sealed class BusyWatcherTicket : IDisposable
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