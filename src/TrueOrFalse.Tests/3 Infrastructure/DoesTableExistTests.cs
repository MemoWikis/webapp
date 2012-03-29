using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core.Infrastructure.Persistence;
using TrueOrFalse.Updates;

namespace TrueOrFalse.Tests
{
    public class DoesTableExistTests : BaseTest
    {
        [Test]
        public void Should_perform_table_exists()
        {
            Assert.That(Resolve<DoesTableExist>().Run("Some_unknown_table"), Is.False);
            Assert.That(Resolve<DoesTableExist>().Run("Category"), Is.True);
        }
    }
}
