public class PathTo
{
    public static string Crawlers() => 
        GetPath("/Web/Crawlers/list-of-crawlers.json");

    public static string EmailTemplate() => 
        GetPath("/Domain/Message/HtmlMessage/HtmlMessageStylesInlined.html");

    public static string EmailTemplate_KnowledgeReport() => 
        GetPath("/Domain/Message/HtmlMessage/KnowledgeReportMsgStylesInlined.cshtml");

    public static string Log_Ignore()
    {
        var path = GetPath("Log.ignore");

        if (path.Contains("Test"))
            path = path.Substring(0, path.IndexOf("TrueOrFalse.Tests")) + "TrueOrFalse.Frontend.Web\\Log.ignore";

        return path;
    }

    private static string GetPath(string fileName)
    {
        if (fileName.Equals("/Web/Crawlers/list-of-crawlers.json"))
            return AppDomain.CurrentDomain.BaseDirectory + "/Web/Crawlers/list-of-crawlers.json";

        if (fileName.Equals("Log.ignore") == false)
            return Path.Combine(App.Environment.WebRootPath, "~/bin/" + fileName);

        if (JobExecute.CodeIsRunningInsideAJob)
            return AppDomain.CurrentDomain.BaseDirectory + "bin/" + fileName;

        return AppDomain.CurrentDomain.BaseDirectory + fileName;
    }
}