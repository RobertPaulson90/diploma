using Diploma.Core.Framework;
using Moq;
using NUnit.Framework;

namespace Diploma.Core.Tests.Framework
{
    [TestFixture]
    public class BusyWatcherTicketTests
    {
        [Test]
        public void Ticket_Should_Once_Call_AddWatch_During_Construct()
        {
            var busyWatcherMock = new Mock<BusyWatcher>(MockBehavior.Strict);
            busyWatcherMock.Setup(x => x.AddWatch());

            var dummy = new BusyWatcherTicket(busyWatcherMock.Object);

            busyWatcherMock.Verify(x => x.AddWatch(), Times.Once);
        }

        [Test]
        public void Ticket_Should_Never_Call_RemoveWatch_During_Construct()
        {
            var busyWatcherMock = new Mock<BusyWatcher>(MockBehavior.Strict);
            busyWatcherMock.Setup(x => x.AddWatch());

            var dummy = new BusyWatcherTicket(busyWatcherMock.Object);

            busyWatcherMock.Verify(x => x.RemoveWatch(), Times.Never);
        }

        [Test]
        public void Ticket_Should_Once_Call_RemoveWatch_During_Disposal()
        {
            var busyWatcherMock = new Mock<BusyWatcher>(MockBehavior.Strict);
            busyWatcherMock.Setup(x => x.AddWatch());
            busyWatcherMock.Setup(x => x.RemoveWatch());

            var dummy = new BusyWatcherTicket(busyWatcherMock.Object);
            dummy.Dispose();

            busyWatcherMock.Verify(x => x.RemoveWatch(), Times.Once);
        }

        [Test]
        public void Ticket_Should_Never_Call_AddWatch_During_Disposal()
        {
            var busyWatcherMock = new Mock<BusyWatcher>(MockBehavior.Strict);
            busyWatcherMock.Setup(x => x.AddWatch());
            busyWatcherMock.Setup(x => x.RemoveWatch());

            var dummy = new BusyWatcherTicket(busyWatcherMock.Object);
            busyWatcherMock.ResetCalls();

            dummy.Dispose();

            busyWatcherMock.Verify(x => x.AddWatch(), Times.Never);
        }
    }
}
