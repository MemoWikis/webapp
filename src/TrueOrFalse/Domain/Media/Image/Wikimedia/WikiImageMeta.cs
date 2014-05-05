using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class WikiImageMeta
    {
        public string ApiHost; 

        public bool ImageNotFound;

        public int PageId;
        public int PageNamespace;
        public string ImageTitle;
        public string ImageRepository;

        public DateTime ImageTimeStamp;

        /// <summary>Original Image Width</summary>
        public int ImageWidth;
        /// <summary>Original Image Height</summary>
        public int ImageHeight;

        /// <summary>Original Image URL</summary>
        public string ImageUrl;
        public string ImageUrlDescription;

        public int ImageThumbWidth;
        public int ImageThumbHeight;
        public string ImageThumbUrl;

        public string User;
        public string UserId;

        public string JSonResult;

        public Stream GetStream()
        {
            var request = (HttpWebRequest)WebRequest.Create(ImageThumbUrl);
            var response = (HttpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
