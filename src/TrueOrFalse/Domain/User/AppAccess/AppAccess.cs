using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

public class AppAccess : DomainEntity
{
    public virtual AppKey AppKey { get; set; }

    public virtual string AppInfoJson { get; set; }

    private AppInfo _appInfo;
    public virtual AppInfo AppInfo
    {
        set
        {
            AppInfoJson= JsonConvert.SerializeObject(value); 
            _appInfo = value;
        }
        get
        {
            if (_appInfo == null)
                _appInfo = JsonConvert.DeserializeObject<AppInfo>(AppInfoJson);

            return _appInfo;
        }
    }

    /// <summary>A login token to memucho. Supersedes username and password.</summary>
    public virtual string AccessToken { get; set; }
    public virtual User User { get; set; }

    /// <summary>Identifies the device, is used for PushMessages.</summary>
    public virtual string DeviceKey { get; set; }

    public AppAccess()
    {
        AppKey = AppKey.MEMO1;
    }
}