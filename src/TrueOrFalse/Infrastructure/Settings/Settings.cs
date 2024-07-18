using Microsoft.Extensions.Configuration;
using TrueOrFalse.Environment;

public class Settings
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [ThreadStatic] public static bool UseWebConfig;

    // LoginProvider properties
    public static string FacebookAppId => _configuration["LoginProvider:FacebookAppId"];
    public static string FacebookAppSecret => _configuration["LoginProvider:FacebookAppSecret"];

    // Stripe properties
    public static string WebhookKeyStripe => _configuration["Stripe:WebhookKeyStripe"];
    public static string StripeSecurityKey => _configuration["Stripe:StripeSecurityKey"];
    public static string StripeBaseUrl => _configuration["Stripe:StripeBaseUrl"];

    // Meilisearch properties
    public static string MeiliSearchUrl => _configuration["Meilisearch:MeiliSearchUrl"];
    public static string MeiliSearchMasterKey => _configuration["Meilisearch:MeiliSearchMasterKey"];

    // General properties
    public static string CanonicalHost => _configuration["General:CanonicalHost"];
    public static string SaltCookie => _configuration["General:SaltCookie"];
    public static int MemuchoUserId => int.Parse(_configuration["General:MemuchoUserId"]);
    public static string Environment => _configuration["General:Environment"];
    public static string UpdateUserSettingsKey => _configuration["General:UpdateUserSettingsKey"];

    // Settings properties
    public static bool WithNHibernateStatistics =>
        bool.Parse(_configuration["Settings:WithNHibernateStatistics"]);

    public static bool DisableAllJobs => bool.Parse(_configuration["Settings:DisableAllJobs"]);

    public static int SessionStateTimeoutInMin =>
        Convert.ToInt32(_configuration["Settings:SessionStateTimeoutInMin"]);

    public static bool UseRedisSession => _configuration.GetValue<bool>("Redis:Use");
    public static string RedisUrl => _configuration["Redis:Url"];

    // Email properties
    public static string EmailFrom => _configuration["Email:EmailFrom"];
    public static string EmailToMemucho => _configuration["Email:EmailToMemucho"];

    // Paths properties
    public static string LomExportPath => _configuration["Paths:LomExportPath"];

    public static string ImagePath =>
        string.IsNullOrEmpty(_configuration["Paths:AbsoluteImagePath"])
            ? Path.Combine(WebHostEnvironmentProvider.GetWebHostEnvironment().ContentRootPath,
                "Images")
            : _configuration["Paths:AbsoluteImagePath"];

    // Connection properties
    public static string ConnectionString => _configuration["General:ConnectionString"];

    //Seq
    public static string SeqUrl => _configuration["General:SeqUrl"];
    public static string SeqApiKey => _configuration["General:SeqApiKey"];


    public static string NuxtSessionStartGuid => _configuration["Nuxt:SessionStartGuid"];

}