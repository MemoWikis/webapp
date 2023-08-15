using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class PathTo
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PathTo(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    public string Crawlers()
    {
        return GetPath("/Web/Crawlers/list-of-crawlers.json",
            _httpContextAccessor.HttpContext,
            _webHostEnvironment);
    }

    public string EmailTemplate()
    {
        return GetPath("/Domain/Message/HtmlMessage/HtmlMessageStylesInlined.html",
            _httpContextAccessor.HttpContext, 
            _webHostEnvironment);
    }

    public string EmailTemplate_KnowledgeReport()
    {
        return GetPath("/Domain/Message/HtmlMessage/KnowledgeReportMsgStylesInlined.cshtml",
            _httpContextAccessor.HttpContext,
            _webHostEnvironment);
    }

    public  string Log_Ignore()
    {
        var path = GetPath("Log.ignore",
            _httpContextAccessor.HttpContext, 
            _webHostEnvironment);

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