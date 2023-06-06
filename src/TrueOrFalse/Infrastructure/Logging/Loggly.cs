using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

public class Loggly
{
    public static void Send(string message, LogglyCategories category)
    {
        var jsonMsg = new JavaScriptSerializer().Serialize(new
        {
            environment = Settings.Environment(),
            message = message,
            category = category.ToString()
        });

        Send(jsonMsg);
    }

    public static void Send(Exception e, LogglyCategories category)
    {
        var jsonMsg = new JavaScriptSerializer().Serialize(new
        {
            environment = Settings.Environment(),
            message = e.Message,
            stackTrace = e.StackTrace,
            source = e.Source,
            category = category.ToString()
        });

        Send(jsonMsg);
    }

    public static void Send(string jsonMsg)
    {
        var logglyKey = Settings.LogglyKey();

        if (String.IsNullOrEmpty(logglyKey))
            return;

        Task.Factory.StartNew(() =>
        {
            var request = WebRequest.Create("https://logs.loggly.com/inputs/" + logglyKey);
            request.Method = "POST";

            string postData = jsonMsg;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        });
    }
}

public enum LogglyCategories
{
    Performance,
}