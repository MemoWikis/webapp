using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Tests;
using TrueOrFalse.Updates;

namespace TrueOrFalse.Tests
{
    public class DbSettingsTests : BaseTest
    {
        [Test]
        public void Should_load_and_save_settings()
        {
            CreateInitialRecord();

            var dbSettingsRepository = Resolve<DbSettingsRepository>();
            Assert.That(dbSettingsRepository.Get().AppVersion, Is.EqualTo(0));
            
            dbSettingsRepository.UpdateAppVersion(25);
            Assert.That(dbSettingsRepository.Get().AppVersion, Is.EqualTo(25));
        }

        private static void CreateInitialRecord()
        {
            var dbSettingsRepository = Resolve<DbSettingsRepository>();
            var dbSettings = new DbSettings();
            dbSettings.AppVersion = 0;
            dbSettingsRepository.Create(dbSettings);
        }
    }
}
