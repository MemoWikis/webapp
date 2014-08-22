using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        [Ignore]
        public void Should_connect_to_message_queue()
        {
            var vhosts = BusMgmt.GetVHosts();
            var queues = BusMgmt.GetQueues();
            BusMgmt.CreateTestQueue();
        }
    }
}
