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

            //$temp:
            //Image "http://commons.wikimedia.org/w/index.php?title=File:OS_Street_View_NC46SW.jpg"
            //mit apiResult: "{\"warnings\":{\"query\":{\"*\":\"Formatting of continuation data will be changing soon. To continue using the current formatting, use the 'rawcontinue' parameter. To begin using the new format, pass an empty string for 'continue' in the initial query.\"}},\"query\":{\"normalized\":[{\"from\":\"File:OS_Street_View_NC46SW.jpg\",\"to\":\"File:OS Street View NC46SW.jpg\"}],\"pages\":{\"10912841\":{\"pageid\":10912841,\"ns\":6,\"title\":\"File:OS Street View NC46SW.jpg\",\"imagerepository\":\"local\",\"imageinfo\":[{\"timestamp\":\"2010-07-17T19:42:54Z\",\"user\":\"OrdnanceSurveyBot\",\"userid\":\"1179188\",\"size\":842955,\"width\":5000,\"height\":5000,\"thumburl\":\"http://upload.wikimedia.org/wikipedia/commons/thumb/9/94/OS_Street_View_NC46SW.jpg/200px-OS_Street_View_NC46SW.jpg\",\"thumbwidth\":200,\"thumbheight\":200,\"url\":\"http://upload.wikimedia.org/wikipedia/commons/9/94/OS_Street_View_NC46SW.jpg\",\"descriptionurl\":\"http://commons.wikimedia.org/wiki/File:OS_Street_View_NC46SW.jpg\",\"sha1\":\"923f6f583bedec3beb855446b65ca16a766f0644\",\"metadata\":null}]}}}}";
            //lässt ihn in folgende Bedingung springen, die dann dazu führt, dass Image nicht gespeichert werden kann:
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
