using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace TrueOrFalse
{
    public class WikiImageMetaLoader
    {
        public static WikiImageMeta Run(string fileNameOrUrl, int imgWidth = 1024, string host = "commons.wikimedia.org")
        {
            fileNameOrUrl = HttpUtility.UrlDecode(fileNameOrUrl);

            if (String.IsNullOrEmpty(fileNameOrUrl))
                return new WikiImageMeta { ImageNotFound = true };

            var fileName = WikiApiUtils.ExtractFileNameFromUrl(fileNameOrUrl);
            var url =
                "http://" + host + "/w/api.php?action=query" +
                "&prop=imageinfo" +
                "&format=json" +
                "&iiprop=timestamp|user|userid|url|size|metadata|sha1" +
                "&iilimit=1" + //return 1 revision
                "&iiextmetadatalanguage=de" +
                "&iiurlwidth=688" +
                "&titles=File:" + HttpUtility.UrlEncode(fileName);
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            SetUserAgent(webRequest);

            string resultString;
            using (var response = webRequest.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                resultString = stream.ReadToEnd();
            }

            var jsonResult = JObject.Parse(resultString);
            var page = jsonResult["query"]["pages"];
            var pageName = ((JObject)page).Properties().Select(p => p.Name).First();

            // if json result does not contain "imageinfo", try api from host same as the image file name host.
            if (((ICollection<string>)page["GetDynamicMemberNames"].Value<ICollection<string>>()).All(x =>
                    x != "imageinfo"))
            {
                if (WikiApiUtils.ExtractDomain(host) == "commons.wikimedia.org")
                {
                    var newTryHost = WikiApiUtils.ExtractDomain(fileNameOrUrl);
                    if (!String.IsNullOrEmpty(newTryHost) && newTryHost != "commons.wikimedia.org")
                        return Run(fileNameOrUrl, imgWidth, newTryHost);
                }

                return new WikiImageMeta { ImageNotFound = true };
            }

            return new WikiImageMeta
            {
                ApiHost = host,
                PageId = page["pageid"].Value<int>(),
                PageNamespace = page["pageName"].Value<int>(),
                User = page["pageName"].Value<string>(),
                UserId = page["userid"].Value<string>(),
                ImageTitle = page["title"].Value<string>(),
                ImageRepository = page["imagerepository"].Value<string>(),
                ImageTimeStamp = DateTime.Parse(page["imageinfo"][0]["timestamp"].Value<string>().Replace('T', ' ')
                    .Replace('Z', ' ')),
                ImageOriginalWidth = page["imageinfo"][0]["width"].Value<int>(),
                ImageOriginalHeight = page["imageinfo"][0]["height"].Value<int>(),
                ImageOriginalUrl = page["imageinfo"][0]["url"].Value<string>(),
                ImageUrlDescription = page["imageinfo"][0]["descriptionurl"].Value<string>(),
                ImageWidth = page["imageinfo"][0]["thumbwidth"].Value<int>(),
                ImageHeight = page["imageinfo"][0]["thumbheight"].Value<int>(),
                ImageUrl = page["imageinfo"][0]["thumburl"].Value<string>(),
                JSonResult = resultString
            };
        }

        public static void SetUserAgent(HttpWebRequest webRequest)
        {
            webRequest.UserAgent = "MemuchoBot/1.1 (http://www.memucho.de/; team@memucho.de)/MemuchoImageLoaderLib/1.1";
        }
    }
}
