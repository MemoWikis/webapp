using System.Xml;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse
{
    public class WikiImageLicenseInfo
    {
        public string AuthorName;
        public string Description;
        public string Markup;
        public DateTime MarkupDownloadDate;
        public string AllRegisteredLicenses;
        public string Notifications;

        public static WikiImageLicenseInfo ParseMarkup(string markup)
        {
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