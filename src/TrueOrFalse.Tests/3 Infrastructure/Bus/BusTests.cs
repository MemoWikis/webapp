using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ObjectDumper;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        [Ignore]
        public void Should_connect_to_message_queue()
        {
            BusMgmt.CreateTestQueue();
            
            var message = new  { Test = "Hello Rabbit" };
            Bus.Get().Publish(message);
        }
    }
}
