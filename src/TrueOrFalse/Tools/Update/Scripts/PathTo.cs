using System;
using System.Web;

public class PathTo
{
    public static string Scrips(string fileName)
    {
        return "Utilities/Update/Scripts/" + fileName;
    }

    public static string Log_Ignore()
    {
        return HttpContext.Current.Server.MapPath("Log.ignore");
    }

    public static string SolrSchema(string fileName)
    {
        return GetPath("Infrastructure/SolrSchemas/" + fileName);
    }

    public static string InvoiceTemplate()
    {
        return GetPath("/Domain/User/Membership/Invoice/InvoiceTemplate.cshtml");
    }

    public static string EmailTemplate()
    {
        return GetPath("/Domain/Message/HtmlMessage/HtmlMessageStylesInlined.html");
    }

    public static string EmailTemplate_TrainingReminder()
    {
        return GetPath("/Domain/Message/HtmlMessage/TrainingReminderForDateMsgStylesInlined.cshtml");
    }

    public static string EmailTemplate_KnowledgeReport()
    {
        return GetPath("/Domain/Message/HtmlMessage/KnowledgeReportMsgStylesInlined.cshtml");
    }

    public static string Crawlers()
    {
        return GetPath("/Web/Crawlers/list-of-crawlers.json");
    }

    private static string GetPath(string fileName)
    {
        if (HttpContext.Current != null)
            return HttpContext.Current.Server.MapPath("~/bin/" + fileName);

        if(JobExecute.CodeIsRunningInsideAJob)
            return AppDomain.CurrentDomain.BaseDirectory + "bin/" + fileName;

        return AppDomain.CurrentDomain.BaseDirectory + fileName;
    }
}

