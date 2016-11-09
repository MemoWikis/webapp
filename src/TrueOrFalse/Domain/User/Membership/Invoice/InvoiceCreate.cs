using System;
using System.Diagnostics;
using System.IO;
using RazorEngine;

public class InvoiceCreate
{
    public static void PDF()
    {
        var invoiceModel = new InvoiceModel
        {
            Id = 12,
            CustomerName = "Firstname Lastname",
            Price = "€12"
        };

        var parsedTemplate = Razor.Parse(File.ReadAllText(PathTo.InvoiceTemplate()), invoiceModel);

        var invoiceFolderPath = Settings.InvoiceFolder();

        var parsedTemplatePath = Path.Combine(
            invoiceFolderPath + invoiceModel.Id + "_" + Guid.NewGuid() + ".html");

        try
        {
            File.WriteAllText(parsedTemplatePath, parsedTemplate);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = Settings.WkHtmlToPdfFolder();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = "cmd.exe";

            var arguments = String.Format(
                @"/c {0} --page-size A4 -q --print-media-type --disable-smart-shrinking ""{1}"" ""{2}""",
                "wkhtmltopdf.exe",
                parsedTemplatePath,
                invoiceFolderPath + invoiceModel.Id + ".pdf");

            startInfo.Arguments = arguments;

            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }
        catch (Exception e)
        {
            Logg.Error(e);
        }
        finally 
        {
            File.Delete(parsedTemplatePath);
        }
    }
}

