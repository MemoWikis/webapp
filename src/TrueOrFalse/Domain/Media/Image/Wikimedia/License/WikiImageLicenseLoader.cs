using System;
using System.Linq;
using System.Xml;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse
{
    public class WikiImageLicenseLoader : IRegisterAsInstancePerLifetime
    {
        public WikiImageLicenseInfo Run(string imageTitle, string apiHost)
        {
            imageTitle = WikiApiUtils.ExtractFileNameFromUrl(imageTitle);

            var url = $"http://{apiHost}/w/index.php?title=File:{imageTitle}&action=raw";

            string markup = "";

            try
            {
                markup = WikiApiUtils.GetWebpage(url);
            }
            catch (Exception e)
            {
                Logg.r().Error(e, "Could not load markup: {url}", url);
            }
            
            markup = RemoveTroublesomeCharacters(markup);

            var parsedImageMarkup = ParseImageMarkup.Run(markup);
            var licenseInfo = new WikiImageLicenseInfo
            {
                AuthorName = parsedImageMarkup.AuthorName,
                Description = parsedImageMarkup.Description,
                Markup = markup,
                MarkupDownloadDate = DateTime.Now,
                AllRegisteredLicenses = parsedImageMarkup.AllRegisteredLicenses,
                Notifications = parsedImageMarkup.Notifications
            };

            return licenseInfo;
        }

        public static string RemoveTroublesomeCharacters(string inString)
        {
            return new string(inString.Where(XmlConvert.IsXmlChar).ToArray());
        }
    }
}