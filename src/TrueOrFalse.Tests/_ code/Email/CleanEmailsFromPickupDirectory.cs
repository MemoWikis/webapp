using System.IO;
using System.Linq;

public class CleanEmailsFromPickupDirectory
{
    public static void Run()
    {
        var files = GetEmailsFromPickupDirectory.Run();

        foreach(var file in files.Where(x => x != "keepFolder.txt"))
            File.Delete(file);
    }
}