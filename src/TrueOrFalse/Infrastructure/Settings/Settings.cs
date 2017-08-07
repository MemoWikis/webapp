using System;
using System.Configuration;
using static System.String;

public class Settings
{
    private static readonly AppSettingsReader _settingReader = new AppSettingsReader();

    [ThreadStatic]
    public static bool UseWebConfig;

    public static string CanonicalHost;

    public static string SolrUrl;
    public static string SolrPath;
    public static string SolrCoresSuffix;
    public static bool GoogleKeyIsSet = false;

    public static string GoogleAnalyticsKey;
    public static string GoogleApiKey;

    public static bool AdvertisementTurnedOn;

    public static string EmailFrom = "team@memucho.de";
    public static string EmailToMemucho = "team@memucho.de";
    
    public static string MemuchoCookie = "memucho";
    public static int MemuchoUserId = 26;
    public static bool WithNHibernateStatistics = true;

    public static int TestSessionQuestionCount = 5;

    public static string LomExportPath;

    private static bool? _developOffline;

    public static bool DevelopOffline()
    {
        if (_developOffline != null)
            return _developOffline.Value;

        var result = OverwrittenConfig.Value("developOffline");

        if (result.HasValue)
            _developOffline = Boolean.Parse(result.Value);
        else
            _developOffline = false;

        return _developOffline.Value;
    }

    /// <summary>Develop / Stage / Live</summary>
    public static string Environment() => OverwrittenConfig.ValueString("environment");

    public static string LogglyKey() => OverwrittenConfig.ValueString("logglyKey");
    public static string BetaCode() => OverwrittenConfig.ValueString("betaCode");

    public static string SignalrUrl() => OverwrittenConfig.ValueString("signalrUrl");
    public static string SignalrUser() => OverwrittenConfig.ValueString("signalrUser");
    public static string SignalrPassword() => OverwrittenConfig.ValueString("signalrPassword");

    public static string UpdateUserSettingsKey() => OverwrittenConfig.ValueString("updateUserSettingsKey");

    public static string InvoiceFolder() => OverwrittenConfig.ValueString("invoiceFolderPath");
    public static string WkHtmlToPdfFolder() => OverwrittenConfig.ValueString("wkHtmlToPdfFolder");

    public static bool DebugUserNHProfiler() => OverwrittenConfig.ValueBool("debugUserNHProfiler");
    public static bool DebugMiniProfiler() => OverwrittenConfig.ValueBool("debugMiniProfiler");

    public static bool DisableAllJobs() => OverwrittenConfig.ValueBool("disableAllJobs");

    public static string RollbarAccessToken => Get<string>("Rollbar.AccessToken");
    public static string RollbarEnvironment => Get<string>("Rollbar.Environment");

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
        GoogleAnalyticsKey = Get<string>("GoogleAnalyticsKey");
        GoogleKeyIsSet = !IsNullOrEmpty(GoogleAnalyticsKey);
        GoogleApiKey = GetValue(OverwrittenConfig.Value("googleApiKey"), "GoogleAnalyticsKey");

        SolrCoresSuffix = GetValue(OverwrittenConfig.Value("solrCoresSuffix"), "SolrCoresSuffix");
        SolrPath = GetValue(OverwrittenConfig.Value("pathToSolr"), "SolrPath");
        SolrUrl = GetValue(OverwrittenConfig.Value("sorlUrl"), "SolrUrl");
        CanonicalHost = GetValue(OverwrittenConfig.Value("CanonicalHost"), "CanonicalHost");
        AdvertisementTurnedOn = bool.Parse(GetValue(OverwrittenConfig.Value("advertisementTurnedOn"), "AdvertisementTurnedOn"));
        LomExportPath = GetValue(OverwrittenConfig.Value("lomExportPath"), "LomExportPath");
    }
}