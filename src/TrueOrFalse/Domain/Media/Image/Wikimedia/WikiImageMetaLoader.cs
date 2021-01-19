using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace TrueOrFalse
{
    public class WikiImageMetaLoader : IRegisterAsInstancePerLifetime
    {
        public WikiImageMeta Run(string fileNameOrUrl, int imgWidth = 1024, string host = "commons.wikimedia.org")
        {
            fileNameOrUrl = HttpUtility.UrlDecode(fileNameOrUrl);
            
            if(String.IsNullOrEmpty(fileNameOrUrl))
                return new WikiImageMeta { ImageNotFound = true };

            var fileName = WikiApiUtils.ExtractFileNameFromUrl(fileNameOrUrl);
            var url =
                "http://" + host + "/w/api.php?action=query" +
                "&prop=imageinfo" +
                "&format=json" +
                "&iiprop=timestamp|user|userid|url|size|metadata|sha1" +
                "&iilimit=1" + //return 1 revision
                "&iiextmetadatalanguage=de" +
                "&iiurlwidth=" + imgWidth +
                "&titles=File:" + HttpUtility.UrlEncode(fileName);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            webRequest.UserAgent = "TrueOrFalseBot/1.0 (http://www.memucho.de/)";

            string resultString;
            using (var response = webRequest.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                resultString = stream.ReadToEnd();
            }

            var jsonResult = Json.Decode(resultString);
            var page = jsonResult.query.pages;
            var pageName = ((ICollection<string>)page.GetDynamicMemberNames()).First();

            // if json result does not contain "imageinfo", try api from host same as the image file name host.
            if (((ICollection<string>) page[pageName].GetDynamicMemberNames()).All(x => x != "imageinfo"))
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
                    PageId = page[pageName].pageid,
                    PageNamespace = page[pageName].ns,
                    User = page[pageName].user,
                    UserId = page[pageName].userid,
                    ImageTitle = page[pageName].title,
                    ImageRepository = page[pageName].imagerepository,
                    ImageTimeStamp = DateTime.Parse(page[pageName].imageinfo[0].timestamp.Replace('T', ' ').Replace('Z', ' ')),
                    ImageOriginalWidth = page[pageName].imageinfo[0].width,
                    ImageOriginalHeight = page[pageName].imageinfo[0].height,
                    ImageOriginalUrl = page[pageName].imageinfo[0].url,
                    ImageUrlDescription = page[pageName].imageinfo[0].descriptionurl,
                    ImageWidth = page[pageName].imageinfo[0].thumbwidth,
                    ImageHeight = page[pageName].imageinfo[0].thumbheight,
                    ImageUrl = page[pageName].imageinfo[0].thumburl,
                    JSonResult = resultString
                };
        }        
    }
}
