using System;
using Diploma.Core.Framework;
using NUnit.Framework;
using Shouldly;

namespace Diploma.Core.Tests.Framework
{
    [TestFixture]
    public class BusyWatcherTests
    {
        [Test]
        public void Obtaining_A_Ticket_Should_Make_BusyWatcher_Busy()
        {
            var busyWatcher = new BusyWatcher();

            var dummy = busyWatcher.GetTicket();

            busyWatcher.IsBusy.ShouldBeTrue();
        }

        [Test]
        public void Obtaining_Multiple_Tickets_Should_Make_BusyWatcher_Busy()
        {
            var busyWatcher = new BusyWatcher();

            var dummy1 = busyWatcher.GetTicket();
            var dummy2 = busyWatcher.GetTicket();

            busyWatcher.IsBusy.ShouldBeTrue();
        }


        [Test]
        public void Obtaining_Multiple_Tickets_Should_Not_Make_BusyWatcher_Busy_After_Disposal()
        {
            var busyWatcher = new BusyWatcher();

            using (var dummy1 = busyWatcher.GetTicket())
            {
            }
            using (var dummy2 = busyWatcher.GetTicket())
            {
            }

            busyWatcher.IsBusy.ShouldBeFalse();
        }

        [Test]
        public void BusyWatcher_Without_Tickets_Should_Be_Not_Busy()
        {
            var busyWatcher = new BusyWatcher();
            
            busyWatcher.IsBusy.ShouldBeFalse();
        }


        [Test]
        public void BusyWatcher_RemoveWatch_Should_Throw_If_Not_Ticket_Present()
        {
            var busyWatcher = new BusyWatcher();

            Should.Throw<InvalidOperationException>(() => busyWatcher.RemoveWatch());
        }
    }
}