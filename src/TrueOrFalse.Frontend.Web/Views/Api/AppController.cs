using System;
using System.Web.Mvc;

public class AppController : BaseController
{
    [HttpPost]
    public JsonResult GetLoginToken(string userName, string password, string appName)
    {
        if(appName != "MEMO1")
            return new JsonResult{Data = new {
                LoginSuccess = false, 
                Message= "Unknown appName"
            }};

        var getAccessTokenResult = GetAppAccessToken.Run(userName, password, AppKey.MEMO1);

        return new JsonResult{
            Data = new {
                LoginSuccess = getAccessTokenResult.LoginSuccess,
                AccessToken = getAccessTokenResult.AccessToken
            }
        };
    }
}
