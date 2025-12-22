using Microsoft.Extensions.Configuration;
public class Settings
{
    private static IConfiguration _configuration = null!;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [ThreadStatic] public static bool UseWebConfig;

    // LoginProvider properties
    public static string FacebookAppId => _configuration["LoginProvider:FacebookAppId"]!;
    public static string FacebookAppSecret => _configuration["LoginProvider:FacebookAppSecret"]!;

    // Stripe properties
    public static string WebhookKeyStripe => _configuration["Stripe:WebhookKeyStripe"]!;
    public static string StripeSecurityKey => _configuration["Stripe:StripeSecurityKey"]!;
    public static string StripeBaseUrl => _configuration["Stripe:StripeBaseUrl"]!;

    // Meilisearch properties
    public static string MeilisearchUrl
    {
        get => _configuration["Meilisearch:MeilisearchUrl"]!;
        set => _configuration["Meilisearch:MeilisearchUrl"] = value;
    }

    public static string MeilisearchMasterKey
    {
        get => _configuration["Meilisearch:MeilisearchMasterKey"]!;
        set => _configuration["Meilisearch:MeilisearchMasterKey"] = value;
    }

    // General properties
    public static string CanonicalHost => _configuration["General:CanonicalHost"]!;
    public static string SaltCookie => _configuration["General:SaltCookie"]!;
    public static int MemoWikisUserId => int.Parse(_configuration["General:MemoWikisUserId"]!);
    public static string Environment => _configuration["General:Environment"]!;
    public static string UpdateUserSettingsKey => _configuration["General:UpdateUserSettingsKey"]!;
    public static string BaseUrl => _configuration["General:BaseUrl"]!;

    public static int SessionStateTimeoutInMin =>
        Convert.ToInt32(_configuration["Settings:SessionStateTimeoutInMin"]);

    public static bool UseRedisSession => _configuration.GetValue<bool>("Redis:Use");
    public static string RedisUrl => _configuration["Redis:Url"]!;

    // Email properties
    public static string EmailFrom => _configuration["Email:EmailFrom"]!;
    public static string EmailToMemoWikis => _configuration["Email:EmailToMemoWikis"]!;
    public static string EmailSmtpUserName => _configuration["Email:SmtpUsername"]!;
    public static string EmailSmtpPassword => _configuration["Email:SmtpPassword"]!;

    public static string ImagePath =>
        (string.IsNullOrEmpty(_configuration["Paths:AbsoluteImagePath"])
            ? Path.Combine(WebHostEnvironmentProvider.GetWebHostEnvironment().ContentRootPath, "Images")
            : _configuration["Paths:AbsoluteImagePath"])!;

    public static string MmapCachePath =>
        (string.IsNullOrEmpty(_configuration["Paths:AbsoluteMmapCachePath"])
            ? Path.Combine(WebHostEnvironmentProvider.GetWebHostEnvironment().ContentRootPath, "viewcache")
            : _configuration["Paths:AbsoluteMmapCachePath"])!;

    public static string PageContentImageBasePath => _configuration["Paths:PageContentImages"]!;
    public static string PageImageBasePath => _configuration["Paths:PageImages"]!;
    public static string QuestionContentImageBasePath => _configuration["Paths:QuestionContentImages"]!;
    public static string QuestionImageBasePath => _configuration["Paths:QuestionImages"]!;

    public static string UserImageBasePath => _configuration["Paths:UserImages"]!;

    // Connection properties
    public static string ConnectionString
    {
        get => _configuration["General:ConnectionString"]!;
        set => _configuration["General:ConnectionString"] = value;
    }

    //Seq
    public static string SeqUrl => _configuration["General:SeqUrl"]!;
    public static string SeqApiKey => _configuration["General:SeqApiKey"]!;

    public static string GoogleClientId => _configuration["Google:ClientId"]!;
    public static string GoogleApplicationName => _configuration["Google:ApplicationName"]!;

    public static string CollaborationTokenSecretKey => _configuration["Collaboration:TokenSecretKey"]!;
    public static string CollaborationHocuspocusSecretKey => _configuration["Collaboration:HocuspocusSecretKey"]!;

    private static string _trackersToIgnoreString => _configuration["General:TrackersToIgnore"]!;
    public static List<string> TrackersToIgnore => _trackersToIgnoreString.Split(',').ToList();

    public static string OpenAIApiKey => _configuration["OpenAI:ApiKey"]!;
    public static string OpenAIModel => _configuration["OpenAI:Model"]!;
    public static string AnthropicApiKey => _configuration["Anthropic:ApiKey"]!;
    public static string AnthropicModel => _configuration["Anthropic:Model"]!;
    public static string AnthropicFallbackModel => _configuration["Anthropic:FallbackModel"] ?? "claude-sonnet-4-20250514";
    public static string AnthropicVersion => _configuration["Anthropic:Version"]!;

    public static int FeaturedPageRootId => int.Parse(_configuration["FeaturedPage:RootId"]!);
    public static int FeaturedPageIntroId => int.Parse(_configuration["FeaturedPage:IntroId"]!);
    public static int FeaturedPageMemoWikisWikiId => int.Parse(_configuration["FeaturedPage:MemoWikisWikiId"]!);
    private static string _featuredMainPageIdsString => _configuration["FeaturedPage:MainPageIds"]!;
    public static List<int> FeaturedMainPageIds => _featuredMainPageIdsString.Split(',').Select(id => int.Parse(id.Trim())).ToList();
    private static string _featuredPopularPageIdsString => _configuration["FeaturedPage:PopularPageIds"]!;
    public static List<int> FeaturedPopularPageIds => _featuredPopularPageIdsString.Split(',').Select(id => int.Parse(id.Trim())).ToList();
    private static string _featuredMemoWikisPageIdsString => _configuration["FeaturedPage:MemoWikisPageIds"]!;
    public static List<int> FeaturedMemoWikisPageIds => _featuredMemoWikisPageIdsString.Split(',').Select(id => int.Parse(id.Trim())).ToList();
    private static string _featuredMemoWikisHelpIdsString => _configuration["FeaturedPage:MemoWikisHelpIds"]!;
    public static List<int> FeaturedMemoWikisHelpIds => _featuredMemoWikisHelpIdsString.Split(',').Select(id => int.Parse(id.Trim())).ToList();
}