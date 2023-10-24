using System.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class Settings
{
    private static readonly AppSettingsReader _settingReader = new();

    [ThreadStatic]
    public static bool UseWebConfig;
    public static string WebhookKeyStripe => OverwrittenConfig.ValueString("WebhookKeyStripe");
    public static string CanonicalHost => GetValue(OverwrittenConfig.Value("canonicalHost"), "CanonicalHost");
    public static string GoogleApiKey => GetValue(OverwrittenConfig.Value("googleApiKey"), "GoogleAnalyticsKey");
    public static string FacebookAppId => OverwrittenConfig.ValueString("FacebookAppId");
    public static string FacebookAppSecret => OverwrittenConfig.ValueString("FacebookAppSecret");
    public static string SaltCookie = "someSalt";
    public static string EmailFrom = "team@memucho.de";
    public static string EmailToMemucho = "team@memucho.de";
    public static string MemuchoCookie = "memucho";
    public static string PersistentLogin = "persistentLogin";
    public static int MemuchoUserId = 26;
    public static bool WithNHibernateStatistics = true;
    public static string LomExportPath => GetValue(OverwrittenConfig.Value("lomExportPath"), "LomExportPath");
    public static bool ShowAdvertisment; 
    public static string MeiliSearchUrl => OverwrittenConfig.ValueString("MeiliSearchUrl");
    public static string MeiliSearcMasterKey => OverwrittenConfig.ValueString("MeiliSearchMasterKey");
    public static string StripeBaseUrl => OverwrittenConfig.ValueString("StripeBaseUrl");

    public static string ImagePath => OverwrittenConfig.ValueString("imagePath"); 

    /// <summary>Develop / Stage / Live</summary>
    public static string Environment(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
        => OverwrittenConfig.ValueString("environment");
    public static string UpdateUserSettingsKey(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
        => OverwrittenConfig.ValueString("updateUserSettingsKey");
    public static bool DisableAllJobs(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
        => OverwrittenConfig.ValueBool("disableAllJobs");
    public static string ConnectionString(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
    {
        var result = OverwrittenConfig.Value("connectionString");
        return result.HasValue ? result.Value : "No value in the Overwritten.config" ;
    }

    private static string GetValue(OverwrittenConfigValueResult overwrittenConfigValueResult, string configKey)
    {
        if (overwrittenConfigValueResult.HasValue)
            return overwrittenConfigValueResult.Value;

        return Get<string>(configKey);
    }

    private static T? Get<T>(string settingKey){
        try
        {
            return (T)_settingReader.GetValue(settingKey, typeof(T));
        }
        catch
        {
            return default;
        }
    }

    public Settings(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
    {
        var environment = Environment(httpContext, webHostEnvironment);
        ShowAdvertisment = environment != "Live" || environment != "Stage";
    }
}