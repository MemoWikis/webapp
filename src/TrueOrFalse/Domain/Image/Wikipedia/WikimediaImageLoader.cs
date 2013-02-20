using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace TrueOrFalse
{
    public class WikimediaImageLoader : IRegisterAsInstancePerLifetime
    {
        public WikimediaImageLoaderResult Run(string fileName)
        {
            var url =
                "http://commons.wikimedia.org/w/api.php?action=query" +
                "&prop=imageinfo" +
                "&format=json" +
                "&iiprop=timestamp%7Cuser%7Cuserid%7Curl%7Csize" +
                "&iilimit=1" +
                "&iiend=20071231235959" +
                "&titles=File%3A" + HttpUtility.UrlEncode(fileName);

            var serializer = new JavaScriptSerializer();
            var webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.UserAgent = "TrueOrFalseBot/1.0 (http://www.richtig-oder-falsch.de/)";

            dynamic jsonResult;
            using (var response = webRequest.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                jsonResult = serializer.Deserialize<WikimediaImageLoaderResult>(stream.ReadToEnd());
            }


            var result = new WikimediaImageLoaderResult();
            return result;
        }
        
    }
}
