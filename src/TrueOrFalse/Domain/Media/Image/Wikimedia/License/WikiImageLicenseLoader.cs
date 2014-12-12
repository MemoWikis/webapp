using System;
using System.IO;
using System.Net;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse
{
    public class WikiImageLicenseLoader : IRegisterAsInstancePerLifetime
    {
        public WikiImageLicenseInfo Run(string imageTitle, string apiHost)
        {
            imageTitle = WikiApiUtils.ExtractFileNameFromUrl(imageTitle);

            var url = String.Format("http://" + apiHost + "/w/index.php?title=File:{0}&action=raw", imageTitle);
            var markup = WikiApiUtils.GetWebpage(url);

            var parsedImageMarkup = ParseImageMarkup.Run(markup);
            var licenseInfo = new WikiImageLicenseInfo
            {
                AuthorName = parsedImageMarkup.AuthorName,
                Description = parsedImageMarkup.Description,
                Markup = markup,
                MarkupDownloadDate = DateTime.Now,
                AllRegisteredLicenses = parsedImageMarkup.AllRegisteredLicenses
            };

            return licenseInfo;
        }
    }
}