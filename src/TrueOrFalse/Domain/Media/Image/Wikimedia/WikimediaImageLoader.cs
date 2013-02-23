using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class WikiMediaImageLoader : IRegisterAsInstancePerLifetime
    {
        private readonly WikimediaImageMetaLoader _metaLoader;

        public WikiMediaImageLoader(WikimediaImageMetaLoader metaLoader){
            _metaLoader = metaLoader;
        }

        public Stream Run(string fileNameOrUrl)
        {
            var metaData = _metaLoader.Run(fileNameOrUrl, 1024);

            var request = (HttpWebRequest)WebRequest.Create(metaData.ImageThumbUrl);
            var response = (HttpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }
    }
}