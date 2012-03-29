using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Tests;

namespace TrueOrFalse.Tests
{
    public class DbSettingsTests : BaseTest
    {

        [Test]
        public void Load_and_save_settings()
        {
            var dbSettingsLoader = Resolve<DbSettingsStorage>();
            var dbSettings = dbSettingsLoader.Get();
            Assert.That(dbSettings.AppVersion, Is.EqualTo(0));
            dbSettings.AppVersion = 1;

            dbSettingsLoader.Update(dbSettings);
        }
    }

   
}
