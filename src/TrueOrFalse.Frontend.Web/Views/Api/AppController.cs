public class AppController : BaseController
{
    public string GetLoginToken(string userName, string password, string appName)
    {
        return "someToken";
    }
}
