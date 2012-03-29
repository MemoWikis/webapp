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
        public void Load_and_save_settings()
        {
            CreateInitialRecord();

            var dbSettingsRepository = Resolve<DbSettingsRepository>();
            var dbSettings = dbSettingsRepository.Get();

            //dbSettingsLoader.Get();
            //Assert.That(dbSettings.AppVersion, Is.EqualTo(0));
            //dbSettings.AppVersion = 1;
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
