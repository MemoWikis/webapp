using System.IO;
using System.Linq;
using NUnit.Framework;

public class Membership_invoice : BaseTest
{
    [Test]
    public void Should_create_invoice()
    {
        var filesToDelete = new DirectoryInfo(Settings.InvoiceFolder()).GetFiles();
        foreach(var file in filesToDelete)
            file.Delete();

        R<InvoiceCreate>().PDF();

        var files = Directory.GetFiles(Settings.InvoiceFolder());
        Assert.That(files.Length, Is.EqualTo(1));
    }   
}
