using Seedworks.Lib.Persistence;

public class AppAccess : DomainEntity
{
    public virtual AppKey AppKey { get; set; }
    public virtual string AccessToken { get; set; }
    public virtual User User { get; set; }

    public AppAccess()
    {
        AppKey = AppKey.MEMO1;
    }
}