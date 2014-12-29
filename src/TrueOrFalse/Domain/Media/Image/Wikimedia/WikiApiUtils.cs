using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class WikiApiUtils
    {
        public static string GetWebpage(string url)
        {
            var webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            webRequest.UserAgent = "TrueOrFalseBot/1.0 (http://www.memucho.de/)";

            string resultString;
            using (var response = webRequest.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                resultString = stream.ReadToEnd();
            }

            return resultString;
        }

        public static string ExtractFileNameFromUrl(string filePath)
        {
            //Wiki file names are case sensitive (including file extension, excluding first character)

            //Must be able to handle both
            //http://commons.wikimedia.org/wiki/File:Liguus_virgineus_01.JPG?uselang=de
            //and
            //http://commons.wikimedia.org/wiki/Hauptseite?uselang=de#mediaviewer/File:Liguus_virgineus_01.JPG

            //remove query string if no file name comes after
            if (filePath.Contains("?")){
                var filePathSplit = filePath.Split(new char[] { '?' }, 2);
                if (!filePathSplit[1].Contains("File:"))
                    filePath = filePathSplit[0];
            }

            //Get file name from mediaviewer url (http://commons.wikimedia.org/wiki/Main_Page#mediaviewer/File:Liguus_virgineus_01.JPG)
            //and details page url (http://commons.wikimedia.org/wiki/File:Liguus_virgineus_01.JPG)  
            if (filePath.Contains("File:"))
                return filePath.Split(new[] { "File:" }, StringSplitOptions.None).Last();

            //Get file name from file url (http://upload.wikimedia.org/wikipedia/commons/0/02/Liguus_virgineus_01.JPG)
            if (filePath.Contains("upload.wikimedia.org"))
                return filePath.Split('/').Last();

            return filePath;

            //http://upload.wikimedia.org/wikipedia/commons/4/4f/Oboe_modern.jpg?uselang=de
        }

        public static string ExtractDomain(string filePath)
        {
            if (!filePath.Contains("."))
                return null;

            filePath = filePath.Replace("http://", "");

            if (filePath.Contains("/"))
                filePath = filePath.Substring(0, filePath.IndexOf("/"));

            if (new[] {"jpg", "jpeg", "png", "gif", "svg"}.Any(x => filePath.EndsWith(x.ToLower())))
                return null;

            return filePath;
        }
    }
}
