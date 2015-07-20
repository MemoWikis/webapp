using System;
using System.Web.Mvc;
using Newtonsoft.Json;

public class AppController : BaseController
{
    [HttpPost]
    public JsonResult GetLoginToken(
        string userName, 
        string password, 
        string appName,
        string appInfoJson,
        string deviceKey)
    {
        if(appName != "MEMO1")
            return new JsonResult{Data = new {
                LoginSuccess = false, 
                Message= "Unknown appName"
            }};

        if (String.IsNullOrEmpty(appInfoJson))
            return new JsonResult{
                Data = new{
                    LoginSuccess = false,
                    Message = "AppInfo Json is null"
                }
            };

        var appInfo = JsonConvert.DeserializeObject<AppInfo>(appInfoJson);

        var getAccessTokenResult = 
            GetAppAccessToken.Run(
                userName, 
                password, 
                AppKey.MEMO1,
                appInfo, 
                deviceKey);

        return new JsonResult{
            Data = new {
                LoginSuccess = getAccessTokenResult.LoginSuccess,
                AccessToken = getAccessTokenResult.AccessToken,
                UserName = getAccessTokenResult.UserName
            }
        };
    }
}
