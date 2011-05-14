using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace TrueOrFalse.Tests
{
    public class GetEmailsFromPickupDirectory
    {
        public static IEnumerable<string> Run()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var mailSettings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            return Directory.GetFiles(mailSettings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation);
        }
    }
}
