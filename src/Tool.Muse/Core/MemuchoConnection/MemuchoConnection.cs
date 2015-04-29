using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace Tool.Muse
{
    public class MemuchoConnection
    {
        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;
        private int _userId;

        public bool IsStarted;

        public async void Start(string userName, string password, string url)
        {
            Log.Info("Starting connection");

            url = "http://" + url;

            var authResult = await AuthenticateUser(userName, password, url);
            var wasAuthSuccessfull = authResult.Item1;
            var authCookie = authResult.Item2;

            if (!wasAuthSuccessfull)
            {
                Log.Info("SignalR", "Invalid login! Could not authenticate");
                return;
            }

            _hubConnection = new HubConnection(url);
            _hubConnection.CookieContainer = new CookieContainer();
            _hubConnection.CookieContainer.Add(authCookie);
            _hubConnection.TraceLevel = TraceLevels.All;
            _hubConnection.TraceWriter = Console.Out;
            _hubConnection.Error += error => { Log.Info(error.Message); };

            _hubProxy = _hubConnection.CreateHubProxy("BrainWavesHub");
            await _hubConnection.Start();

            IsStarted = true;
        }

        public async void SendConcentrationLevel(string level){
            if(IsStarted)
                await _hubProxy.Invoke("SendConcentration", level, _userId);
        }

        public async void SendMellowLevel(string level){
            if (IsStarted)
                await _hubProxy.Invoke("SendMellow", level, _userId);
        }

        public async void SendDisconnected(){
            if (IsStarted)
                await _hubProxy.Invoke("DisconnectEEG", _userId);
        }

        private async Task<Tuple<bool, Cookie>> AuthenticateUser(string userName, string password, string domain)
        {
            var request = WebRequest.Create(domain + "/Welcome/RemoteLogin") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();

            var authCredentials = "UserName=" + userName + "&Password=" + password;
            byte[] bytes = Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;

            using (var requestStream = await request.GetRequestStreamAsync())
                requestStream.Write(bytes, 0, bytes.Length);

            Cookie authCookie;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    var type = new {UserId = -1};
                    type = JsonConvert.DeserializeAnonymousType(responseText, type);
                    Log.Info("Remotly logged in with user id: " + type.UserId);
                    _userId = type.UserId;
                }                

                Log.Info("SignalR", "Auth Response.StatusCode: " + response.StatusCode);
                authCookie = response.Cookies[FormsAuthentication.FormsCookieName];
            }

            return new Tuple<bool, Cookie>(authCookie != null, authCookie);
        }
    }
}
