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
            var dbSettingsLoader = Resolve<DbSettingsLoader>();
            var dbSettings = dbSettingsLoader.Get();
            Assert.That(dbSettings.Integer1.Value, Is.EqualTo(99));
            Assert.That(dbSettings.AppVersion.Value, Is.EqualTo(0));
            dbSettings.AppVersion.Value = 1;
            dbSettings.Integer1.Value = 100;

            dbSettingsLoader.Update(dbSettings);
        }
    }

   
}
