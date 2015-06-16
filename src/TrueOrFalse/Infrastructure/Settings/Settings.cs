using System;
using System.Configuration;

public class Settings
{
    private static readonly AppSettingsReader _settingReader = new AppSettingsReader();

    [ThreadStatic] public static bool UseWebConfig;

    public static string SolrUrl;
    public static string SolrPath;
    public static string SolrCoresSuffix;
    public static bool GoogleKeyIsSet = false;
    public static string GoogleKey;

    public static string EmailFrom = "team@memucho.de";
    public static string EmailTo = "team@memucho.de";
    
    public static string MemuchoCookie = "memucho";

    public static bool DevelopOffline()
    {
        var result = OverwrittenConfig.Value("developOffline");
        return result.HasValue && Boolean.Parse(result.Value);
    }

    /// <summary>Develop / Stage / Live</summary>
    public static string Environment(){ return OverwrittenConfig.ValueString("environment"); }
    public static string LogglyKey(){ return OverwrittenConfig.ValueString("logglyKey"); }
    public static string BetaCode(){ return OverwrittenConfig.ValueString("betaCode"); }

    public static string SignalrUrl() { return OverwrittenConfig.ValueString("signalrUrl"); }
    public static string SignalrUser() { return OverwrittenConfig.ValueString("signalrUser"); }
    public static string SignalrPassword() { return OverwrittenConfig.ValueString("signalrPassword"); }

    public static string InvoiceFolder() { return OverwrittenConfig.ValueString("invoiceFolderPath"); }

    public static string WkHtmlToPdfFolder() { return OverwrittenConfig.ValueString("wkHtmlToPdfFolder"); }

    public static string ConnectionString()
    {
        var result = OverwrittenConfig.Value("connectionString");
        return result.HasValue ? result.Value : ConfigurationManager.ConnectionStrings["main"].ConnectionString;
    }

    private static string GetValue(OverwrittenConfigValueResult overwrittenConfigValueResult, string configKey)
    {
        if (overwrittenConfigValueResult.HasValue)
            return overwrittenConfigValueResult.Value;

        return Get<string>(configKey);
    }

    private static T Get<T>(string settingKey){
        try
        {
            return (T)_settingReader.GetValue(settingKey, typeof(T));
        }
        catch
        {
            return default(T);
        }
    }

    static Settings()
    {
        GoogleKey = Get<string>("GoogleAnalyticsKey");
        GoogleKeyIsSet = !String.IsNullOrEmpty(GoogleKey);
        SolrCoresSuffix = GetValue(OverwrittenConfig.Value("solrCoresSuffix"), "SolrCoresSuffix");
        SolrPath = GetValue(OverwrittenConfig.Value("pathToSolr"), "SolrPath");
        SolrUrl = GetValue(OverwrittenConfig.Value("sorlUrl"), "SolrUrl");
    }
}