using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace TrueOrFalse.Tests
{
    public class CleanEmailsFromPickupDirectory
    {
        public static void Run()
        {
            var files = GetEmailsFromPickupDirectory.Run();

            foreach(var file in files){
                File.Delete(file);
            }
        }
    }
}
