using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Configuration;

public class GetEmailsFromPickupDirectory
{
    public static IEnumerable<string> Run()
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        var mailSettings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
        return Directory.GetFiles(mailSettings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation);
    }
}