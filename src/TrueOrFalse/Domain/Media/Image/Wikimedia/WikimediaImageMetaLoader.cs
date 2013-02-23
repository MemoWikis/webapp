using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace TrueOrFalse
{
    public class WikimediaImageMetaLoader : IRegisterAsInstancePerLifetime
    {
        public WikimediaImageMetaLoaderResult Run(string fileNameOrUrl, int thumbUrlWidth = 1024)
        {
            if(String.IsNullOrEmpty(fileNameOrUrl))
                return new WikimediaImageMetaLoaderResult { ImageNotFound = true };

            var fileName = ParseFileName(fileNameOrUrl);
            var url =
                "http://commons.wikimedia.org/w/api.php?action=query" +
                "&prop=imageinfo" +
                "&format=json" +
                "&iiprop=timestamp%7Cuser%7Cuserid%7Curl%7Csize" +
                "&iilimit=1" +
                "&iiurlwidth=" + thumbUrlWidth +
                "&iiend=20071231235959" +
                "&titles=File%3A" + HttpUtility.UrlEncode(fileName);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            webRequest.UserAgent = "TrueOrFalseBot/1.0 (http://www.richtig-oder-falsch.de/)";

            string resultString;
            using (var response = webRequest.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                resultString = stream.ReadToEnd();
            }

            var jsonResult = Json.Decode(resultString);
            var page = jsonResult.query.pages;
            var pageName = ((ICollection<string>)page.GetDynamicMemberNames()).First();

            if (((ICollection<string>) page[pageName].GetDynamicMemberNames()).All(x => x != "imageinfo"))
                return new WikimediaImageMetaLoaderResult{ImageNotFound = true};

            return new WikimediaImageMetaLoaderResult
                {
                    PageId = page[pageName].pageid,
                    PageNamespace = page[pageName].ns,
                    ImageTitle = page[pageName].title,
                    ImageRepository = page[pageName].imagerepository,
                    ImageTimeStamp = DateTime.Parse(page[pageName].imageinfo[0].timestamp.Replace('T', ' ').Replace('Z', ' ')),
                    ImageWidth = page[pageName].imageinfo[0].width,
                    ImageHeight = page[pageName].imageinfo[0].height,
                    ImageUrl = page[pageName].imageinfo[0].url,
                    ImageUrlDescription = page[pageName].imageinfo[0].descriptionurl,
                    ImageThumbWidth = page[pageName].imageinfo[0].thumbwidth,
                    ImageThumbHeight = page[pageName].imageinfo[0].thumbheight,
                    ImageThumbUrl = page[pageName].imageinfo[0].thumburl
                };
        }

        private string ParseFileName(string filePath)
        {
            if (filePath.Contains("File:")){
                return filePath.Split(new[] { "File:" }, StringSplitOptions.None)[1];
            }

            return filePath;
        }
        
    }
}
