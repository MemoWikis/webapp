using System;
using System.IO;
using System.Net;

namespace TrueOrFalse
{
    public class WikiImageLicenceLoader : IRegisterAsInstancePerLifetime
    {
        private readonly WikiImageMetaLoader _metaDataLoader;

        public WikiImageLicenceLoader(WikiImageMetaLoader metaDataLoader)
        {
            _metaDataLoader = metaDataLoader;
        }

        public WikiImageLicenceInfo Run(string fileName)
        {
            var metaData = _metaDataLoader.Run(fileName);

            var url = String.Format("http://commons.wikimedia.org/w/index.php?title=File:{0}&action=raw", fileName);
            var page = WikiApiUtils.GetWebpage(url);

            //webseite parsen

            //lizenz objekt zurückgeben
            //lizenz objekt soll attribution beeinhalten

            return new WikiImageLicenceInfo();
        }
    }
}