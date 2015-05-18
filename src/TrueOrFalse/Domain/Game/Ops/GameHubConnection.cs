using System;
using System.Net;
using System.Text;
using System.Web.Security;
using Microsoft.AspNet.SignalR.Client;

public class GameHubConnection : IRegisterAsInstancePerLifetime
{
    private HubConnection _hubConnection;
    private IHubProxy _hubProxy;
    private bool _isConnected;

    private void Connect()
    {
        var userName = Settings.SignalrUser();
        var password = Settings.SignalrPassword();
        var url = Settings.SignalrUrl();

        var authResult = AuthenticateUser(userName, password, url);
        var wasAuthSuccessfull = authResult.Item1;
        var authCookie = authResult.Item2;

        if (!wasAuthSuccessfull)
        {
            Logg.r().Error("Invalid login! Could not authenticate");
        }

        _hubConnection = new HubConnection(url);
        _hubConnection.CookieContainer = new CookieContainer();
        _hubConnection.CookieContainer.Add(authCookie);
        _hubConnection.TraceLevel = TraceLevels.All;
        _hubConnection.TraceWriter = Console.Out;
        _hubConnection.Error += error => { Logg.r().Error(error, "Error connecting to hub"); };

        _hubProxy = _hubConnection.CreateHubProxy("GameHub");

        _hubConnection.Start();

        _isConnected = true;
    }

    public void SendNextRound(int gameId)
    {
        Send(() => { _hubProxy.Invoke("NextRound", gameId); }); 
}

    public void SendCompleted(int gameId)
    {   
        Send(()=> { _hubProxy.Invoke("Completed", gameId); }); 
    }

    public void Send(Action action)
    {
        try
        {
            if (!_isConnected)
                Connect();

            action();
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Error sending to hub");
        }
    }

    private Tuple<bool, Cookie> AuthenticateUser(string userName, string password, string domain)
    {
        var request = WebRequest.Create(domain + "/Welcome/RemoteLogin") as HttpWebRequest;
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.CookieContainer = new CookieContainer();

        var authCredentials = "UserName=" + userName + "&Password=" + password;
        byte[] bytes = Encoding.UTF8.GetBytes(authCredentials);
        request.ContentLength = bytes.Length;

        using (var requestStream = request.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);

        Cookie authCookie;
        using (var response = request.GetResponse() as HttpWebResponse)
        authCookie = response.Cookies[FormsAuthentication.FormsCookieName];

        return new Tuple<bool, Cookie>(authCookie != null, authCookie);
    }
}