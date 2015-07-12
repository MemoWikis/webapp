using System;

public class GetAppAccessToken
{
    public static GetAppAccessTokenResult Run(
        string userName, 
        string password, 
        AppKey appKey,
        AppInfo appInfo,
        string deviceKey)
    {
        var credentialsAreValid = Sl.R<CredentialsAreValid>();

        if (!credentialsAreValid.Yes(userName, password)){ 
            return new GetAppAccessTokenResult{
                LoginSuccess = false,
                AccessToken = ""
            };
        }

        var appAccessRepo = Sl.R<AppAccessRepo>();
        var appAccess = appAccessRepo.GetByUser(credentialsAreValid.User, appKey);

        if (appAccess == null)
        {
            appAccess = new AppAccess{
                AppKey = appKey,
                AccessToken = Guid.NewGuid().ToString(),
                User = credentialsAreValid.User,
                AppInfo = appInfo,
                DeviceKey = deviceKey
            };

            appAccessRepo.Create(appAccess);
        }

        return new GetAppAccessTokenResult()
        {
            LoginSuccess = true,
            AccessToken = appAccess.AccessToken,
            UserName = appAccess.User.Name
        };
    }
}

public class GetAppAccessTokenResult
{
    public string UserName;
    public bool LoginSuccess;
    public string AccessToken;
}
