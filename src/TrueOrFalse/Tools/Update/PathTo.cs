using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class PathTo
{
    public static string Crawlers()
    {
        return GetPath("/Web/Crawlers/list-of-crawlers.json");
    }

    public static string EmailTemplate()
    {
        return GetPath("/Domain/Message/HtmlMessage/HtmlMessageStylesInlined.html");
    }

    public static string EmailTemplate_KnowledgeReport()
    {
        return GetPath("/Domain/Message/HtmlMessage/KnowledgeReportMsgStylesInlined.cshtml");
    }

    public static string Log_Ignore()
    {
        var path = GetPath("Log.ignore");

        if (path.Contains("Test"))
        {
            path = path.Substring(0, path.IndexOf("TrueOrFalse.Tests")) + "TrueOrFalse.Frontend.Web\\Log.ignore";
        }

        return path;
    }

    public static string Scrips(string fileName)
    {
        return "Utilities/Update/Scripts/" + fileName;
    }

    private static string GetPath(string fileName, HttpContext? httpContext, IWebHostEnvironment environment)
    {
        if (httpContext != null && !fileName.Equals("Log.ignore"))
        {
            return Path.Combine(environment.WebRootPath, "~/bin/" + fileName);
        }

        if (JobExecute.CodeIsRunningInsideAJob)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "bin/" + fileName;
        }

        return AppDomain.CurrentDomain.BaseDirectory + fileName;
    }
}