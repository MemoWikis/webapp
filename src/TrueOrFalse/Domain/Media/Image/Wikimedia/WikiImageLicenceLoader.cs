using System;

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

            //webseite rausfinden
            var url = String.Format("http://commons.wikimedia.org/w/index.php?title=File:{0}&action=raw",);
            //webseite laden
            //webseite parsen



            //lizenz objekt zurückgeben
            //lizenz objekt soll attribution beeinhalten

            return new WikiImageLicenceInfo();
        }
    }
}