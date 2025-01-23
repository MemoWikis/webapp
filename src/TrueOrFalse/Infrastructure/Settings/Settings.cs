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
    public static int MemoWikisUserId => int.Parse(_configuration["General:MemoWikisUserId"]);
    public static string Environment => _configuration["General:Environment"];
    public static string UpdateUserSettingsKey => _configuration["General:UpdateUserSettingsKey"];

    public static int SessionStateTimeoutInMin =>
        Convert.ToInt32(_configuration["Settings:SessionStateTimeoutInMin"]);

    public static bool UseRedisSession => _configuration.GetValue<bool>("Redis:Use");
    public static string RedisUrl => _configuration["Redis:Url"];

    // Email properties
    public static string EmailFrom => _configuration["Email:EmailFrom"];
    public static string EmailToMemoWikis => _configuration["Email:EmailToMemoWikis"];

    public static string ImagePath =>
        string.IsNullOrEmpty(_configuration["Paths:AbsoluteImagePath"])
            ? Path.Combine(WebHostEnvironmentProvider.GetWebHostEnvironment().ContentRootPath,
                "Images")
            : _configuration["Paths:AbsoluteImagePath"];

    public static string PageContentImageBasePath => _configuration["Paths:PageContentImages"];
    public static string PageImageBasePath => _configuration["Paths:PageImages"];
    public static string QuestionContentImageBasePath => _configuration["Paths:QuestionContentImages"];
    public static string QuestionImageBasePath => _configuration["Paths:QuestionImages"];

    public static string UserImageBasePath => _configuration["Paths:UserImages"];

    // Connection properties
    public static string ConnectionString => _configuration["General:ConnectionString"];

    //Seq
    public static string SeqUrl => _configuration["General:SeqUrl"];
    public static string SeqApiKey => _configuration["General:SeqApiKey"];


    public static string GoogleClientId => _configuration["Google:ClientId"];
    public static string GoogleApplicationName => _configuration["Google:ApplicationName"];

    public static string CollaborationTokenSecretKey => _configuration["Collaboration:TokenSecretKey"];
    public static string CollaborationHocuspocusSecretKey => _configuration["Collaboration:HocuspocusSecretKey"];

    private static string _trackersToIgnoreString => _configuration["General:TrackersToIgnore"];
    public static List<string> TrackersToIgnore => _trackersToIgnoreString.Split(',').ToList();

    public static string OpenAIApiKey => _configuration["OpenAI:ApiKey"];
    public static string OpenAIModel => _configuration["OpenAI:Model"];
    public static string AnthropicApiKey => _configuration["Anthropic:ApiKey"];
    public static string AnthropicModel => _configuration["Anthropic:Model"];
    public static string AnthropicVersion => _configuration["Anthropic:Version"];

}