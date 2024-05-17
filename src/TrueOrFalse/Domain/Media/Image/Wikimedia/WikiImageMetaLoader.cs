using System.Globalization;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace TrueOrFalse
{
    public class WikiImageMetaLoader
    {
        public static WikiImageMeta Run(
            string fileNameOrUrl,
            int imgWidth = 1024,
            string host = "commons.wikimedia.org")
        {
            fileNameOrUrl = HttpUtility.UrlDecode(fileNameOrUrl);

            if (string.IsNullOrEmpty(fileNameOrUrl))
                return new WikiImageMeta { ImageNotFound = true };

            var fileName = WikiApiUtils.ExtractFileNameFromUrl(fileNameOrUrl);
            var url =
                $"https://{host}/w/api.php?action=query&prop=imageinfo&format=json" +
                "&iiprop=timestamp|user|userid|url|size|metadata|sha1" +
                "&iilimit=1&iiextmetadatalanguage=de" +
                $"&iiurlwidth={imgWidth}" +
                $"&titles=File:{HttpUtility.UrlEncode(fileName)}";

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            SetUserAgent(webRequest);

            string resultString;
            using (var response = webRequest.GetResponse())
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                resultString = stream.ReadToEnd();
            }

            var jsonResult = JObject.Parse(resultString);
            var page = jsonResult["query"]["pages"].First;

            // Handle no imageinfo in JSON
            if (page.First["imageinfo"] == null)
            {
                var newTryHost = WikiApiUtils.ExtractDomain(fileNameOrUrl);
                if (!string.IsNullOrEmpty(newTryHost) && newTryHost != host)
                    return Run(fileNameOrUrl, imgWidth, newTryHost);

                return new WikiImageMeta { ImageNotFound = true };
            }

            var imageInfo = page.First["imageinfo"].First;
            var timestamp = (string)imageInfo["timestamp"];

            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTime dateTime = DateTime.ParseExact(timestamp, "MM/dd/yyyy HH:mm:ss", culture);

            return new WikiImageMeta
            {
                ApiHost = host,
                PageId = (int)page.First["pageid"],
                PageNamespace = (int)page.First["ns"],
                User = (string)imageInfo["user"],
                UserId = (string)imageInfo["userid"],
                ImageTitle = (string)page.First["title"],
                ImageRepository = (string)page.First["imagerepository"],
                ImageTimeStamp = dateTime,
                ImageOriginalWidth = (int)imageInfo["width"],
                ImageOriginalHeight = (int)imageInfo["height"],
                ImageOriginalUrl = (string)imageInfo["url"],
                ImageUrlDescription = (string)imageInfo["descriptionurl"],
                ImageWidth = imgWidth,
                ImageHeight = (int)imageInfo["height"] * imgWidth / (int)imageInfo["width"],
                ImageUrl = (string)imageInfo["thumburl"],
                JSonResult = resultString
            };
        }

        public static void SetUserAgent(HttpWebRequest webRequest)
        {
            webRequest.UserAgent =
                "MemuchoBot/1.1 (http://www.memucho.de/; team@memucho.de)/MemuchoImageLoaderLib/1.1";
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }
    }
}
