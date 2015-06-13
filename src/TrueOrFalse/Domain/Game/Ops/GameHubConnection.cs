using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

public class GameHubConnection : IRegisterAsInstancePerLifetime, IDisposable
{
    private HubConnection _hubConnection;
    private IHubProxy _hubProxy;
    private bool _isConnected;

    private void Connect()
    {
        lock("CD235F68-085D-46F4-A8B6-0D93AADB8B97")
        {
            if (_isConnected)
                return;

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

            var task = _hubConnection.Start();
            task.Wait();

            _isConnected = true;            
        }
    }

    public void SendCreated(int gameId)
    {
        Send(() => { _hubProxy.Invoke("Created", gameId).Wait(); });
    }

    public void SendNextRound(int gameId)
    {
        Send(() => { _hubProxy.Invoke("NextRound", gameId).Wait(); }); 
    }

    public void SendCompleted(int gameId)
    {   
        Send(() => { _hubProxy.Invoke("Completed", gameId).Wait(); }); 
    }

    public void SendNeverStarted(int gameId)
    {
        Send(() => { _hubProxy.Invoke("NeverStarted", gameId).Wait(); });
    }

    public void SendStart(int gameId)
    {
        Send(() => { _hubProxy.Invoke("Started", gameId).Wait(); });
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
        {
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                string responseText = reader.ReadToEnd();
                var type = new { UserId = -1 };
                type = JsonConvert.DeserializeAnonymousType(responseText, type);
                Logg.r().Information("GameHubConnection: Remotly logged in with user id: " + type.UserId);
            }

            Logg.r().Information("GameHubConnection: Auth Response.StatusCode: " + response.StatusCode);
            authCookie = response.Cookies[FormsAuthentication.FormsCookieName];
        }

        return new Tuple<bool, Cookie>(authCookie != null, authCookie);
    }

    public void Dispose()
    {
        if(_hubConnection != null)
            _hubConnection.Dispose();
    }
}