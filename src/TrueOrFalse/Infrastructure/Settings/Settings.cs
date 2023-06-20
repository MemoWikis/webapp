﻿using System;
using System.Configuration;

public class Settings
{
    private static readonly AppSettingsReader _settingReader = new AppSettingsReader();

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

    private static bool? _useMeiliSearch;
    public static string MeiliSearchUrl;
    public static string MeiliSearcMasterKey;

    public static string StripeBaseUrl;

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

    public static bool UseMeiliSearch()
    {
        if (_useMeiliSearch != null)
            return _useMeiliSearch.Value;

        var result = OverwrittenConfig.Value("useMeiliSearch");

        if (result.HasValue)
            _useMeiliSearch = Boolean.Parse(result.Value);
        else
            _useMeiliSearch = false;

        return _useMeiliSearch.Value;
    }

    public static bool InitEntityCacheViaJobScheduler()
    {
        var result = OverwrittenConfig.Value("initEntityCacheViaJobScheduler");

        return result.HasValue && Boolean.Parse(result.Value);
    }

    /// <summary>Develop / Stage / Live</summary>
    public static string Environment() => OverwrittenConfig.ValueString("environment");

    public static string LogglyKey() => OverwrittenConfig.ValueString("logglyKey");
    public static string BetaCode() => OverwrittenConfig.ValueString("betaCode");

    public static string UpdateUserSettingsKey() => OverwrittenConfig.ValueString("updateUserSettingsKey");

    public static string InvoiceFolder() => OverwrittenConfig.ValueString("invoiceFolderPath");
    public static string WkHtmlToPdfFolder() => OverwrittenConfig.ValueString("wkHtmlToPdfFolder");
    public static bool DebugEnableMiniProfiler() => OverwrittenConfig.ValueBool("debugEnableMiniProfiler");

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
        GoogleApiKey = GetValue(OverwrittenConfig.Value("googleApiKey"), "GoogleAnalyticsKey");
        MeiliSearchUrl =  OverwrittenConfig.ValueString("MeiliSearchUrl");
        MeiliSearcMasterKey = OverwrittenConfig.ValueString("MeiliSearchMasterKey");
        CanonicalHost = GetValue(OverwrittenConfig.Value("canonicalHost"), "CanonicalHost");
        AdvertisementTurnedOn = bool.Parse(GetValue(OverwrittenConfig.Value("advertisementTurnedOn"), "AdvertisementTurnedOn"));
        LomExportPath = GetValue(OverwrittenConfig.Value("lomExportPath"), "LomExportPath");
        SecurityKeyStripe = OverwrittenConfig.ValueString("SecurityKeyStripe");
        WebhookKeyStripe = OverwrittenConfig.ValueString("WebhookKeyStripe");
        ShowAdvertisment = Environment() != "Live" || Environment() != "Stage";
        StripeBaseUrl = OverwrittenConfig.ValueString("StripeBaseUrl");
    }
}