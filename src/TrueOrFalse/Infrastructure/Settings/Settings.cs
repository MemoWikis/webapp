using System.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class Settings
{
    private static readonly AppSettingsReader _settingReader = new();

    [ThreadStatic]
    public static bool UseWebConfig;
    public static string SecurityKeyStripe { get; set; }
    public static string WebhookKeyStripe { get; set; }

    public static string CanonicalHost;

   // public static bool GoogleKeyIsSet = false;

   // public static string GoogleAnalyticsKey;
    public static string GoogleApiKey;

    public static bool AdvertisementTurnedOn;

    public static string EmailFrom = "team@memucho.de";
    public static string EmailToMemucho = "team@memucho.de";
    
    public static string MemuchoCookie = "memucho";
    public static int MemuchoUserId = 26;
    public static bool WithNHibernateStatistics = true;

    public static int TestSessionQuestionCount = 5;

    public static string LomExportPath;

    public static bool ShowAdvertisment; 

    private static bool? _developOffline;

    public static string MeiliSearchUrl;
    public static string MeiliSearcMasterKey;

    public static string StripeBaseUrl;

    public static bool InitEntityCacheViaJobScheduler(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
    {
        var result = new OverwrittenConfig().Value("initEntityCacheViaJobScheduler");

        return result.HasValue && Boolean.Parse(result.Value);
    }

    /// <summary>Develop / Stage / Live</summary>
    public static string Environment(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
        => new OverwrittenConfig().ValueString("environment");
    public static string UpdateUserSettingsKey(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
        => new OverwrittenConfig().ValueString("updateUserSettingsKey");
    public static bool DisableAllJobs(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
        => new OverwrittenConfig().ValueBool("disableAllJobs");
    public static string RollbarAccessToken => Get<string>("Rollbar.AccessToken");
    public static string RollbarEnvironment => Get<string>("Rollbar.Environment");

    public static string ConnectionString(HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
    {
        var result = new OverwrittenConfig().Value("connectionString");
        return result.HasValue ? result.Value : ConfigurationManager.ConnectionStrings["main"].ConnectionString;
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
        var overwrittenConfig = new OverwrittenConfig();
        var environment = Environment(httpContext, webHostEnvironment); 

        GoogleApiKey = GetValue(overwrittenConfig.Value("googleApiKey"), "GoogleAnalyticsKey");
        MeiliSearchUrl = overwrittenConfig.ValueString("MeiliSearchUrl");
        MeiliSearcMasterKey = overwrittenConfig.ValueString("MeiliSearchMasterKey");
        CanonicalHost = GetValue(overwrittenConfig.Value("canonicalHost"), "CanonicalHost");
        AdvertisementTurnedOn = bool.Parse(GetValue(overwrittenConfig.Value("advertisementTurnedOn"), "AdvertisementTurnedOn"));
        LomExportPath = GetValue(overwrittenConfig.Value("lomExportPath"), "LomExportPath");
        SecurityKeyStripe = overwrittenConfig.ValueString("SecurityKeyStripe");
        WebhookKeyStripe = overwrittenConfig.ValueString("WebhookKeyStripe");
        ShowAdvertisment = environment != "Live" || environment != "Stage";
        StripeBaseUrl = overwrittenConfig.ValueString("StripeBaseUrl");
    }
}