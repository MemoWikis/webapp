using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

public class GetEmailsFromPickupDirectory
{
    public static IEnumerable<string> Run()
    {
        //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //var mailSettings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
        //return Directory.GetFiles(mailSettings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation);

        throw new NotImplementedException();
    }

    public static List<FileSystemInfo> GetAsDateSortedList()
    {
        //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //var mailSettings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
        //DirectoryInfo di = new DirectoryInfo(mailSettings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation);
        //FileSystemInfo[] files = di.GetFileSystemInfos();
        //var orderedFiles = files
        //    .OrderBy(f => f.CreationTime)
        //    .ToList();
        //return orderedFiles;

        throw new NotImplementedException();
    }
}