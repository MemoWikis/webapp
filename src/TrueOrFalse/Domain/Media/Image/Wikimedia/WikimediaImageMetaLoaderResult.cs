using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class WikimediaImageMetaLoaderResult
    {
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
    }
}
