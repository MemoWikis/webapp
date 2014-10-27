using System;
using System.IO;
using System.Net;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse
{
    public class WikiImageLicenceLoader : IRegisterAsInstancePerLifetime
    {
        public WikiImageLicenseInfo Run(string imageTitle, string apiHost)
        {
            imageTitle = WikiApiUtils.ExtractFileNameFromUrl(imageTitle);

            var url = String.Format("http://" + apiHost + "/w/index.php?title=File:{0}&action=raw", imageTitle);
            var markup = WikiApiUtils.GetWebpage(url);

            var parsedImageMakup = ParseImageMarkup.Run(markup);
            var licenseInfo = new WikiImageLicenseInfo
            {
                AuthorName = parsedImageMakup.AuthorName,
                Description = parsedImageMakup.Description,
                Markup = markup
            };

            return licenseInfo;
        }
    }
}