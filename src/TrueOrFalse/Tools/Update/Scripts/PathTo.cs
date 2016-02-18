using System;
using System.Web;

public class PathTo
{
    public static string Scrips(string fileName)
    {
        return "Utilities/Update/Scripts/" + fileName;
    }

    public static string SolrSchema(string fileName)
    {
        return GetPath("Infrastructure/SolrSchemas/" + fileName);
    }

    public static string InvoiceTemplate()
    {
        return GetPath("/Domain/User/Membership/Invoice/InvoiceTemplate.cshtml");
    }

    public static string EmailTemplate_TrainingReminder()
    {
        return GetPath("/Domain/Message/Types/TrainingReminderMsg.cshtml");
    }

    private static string GetPath(string fileName)
    {
        if (HttpContext.Current != null)
            return HttpContext.Current.Server.MapPath("bin/" + fileName);

        return AppDomain.CurrentDomain.BaseDirectory + fileName;        
    }
}

