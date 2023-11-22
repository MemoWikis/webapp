using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Maintenance
{
    public class LoadImageMarkups : IRegisterAsInstancePerLifetime
    {
        private readonly ImageMetaDataReadingRepo _imgReadingRepo;
        private readonly WikiImageLicenseLoader _wikiImageLicenseLoader;

        public LoadImageMarkups(
            ImageMetaDataReadingRepo imgReadingRepo,
            WikiImageLicenseLoader wikiImageLicenseLoader)
        {
            _imgReadingRepo = imgReadingRepo;
            _wikiImageLicenseLoader = wikiImageLicenseLoader;
        }

        public void Run(ImageMetaData imageMetaData, bool loadMarkupFromWikipedia = true)
        {
            if(imageMetaData.Source != ImageSource.WikiMedia) return;

            var fileName = imageMetaData.SourceUrl.Split('/').Last();

            var licenseInfo = loadMarkupFromWikipedia ? 
                _wikiImageLicenseLoader.Run(fileName, imageMetaData.ApiHost) : 
                WikiImageLicenseInfo.ParseMarkup(imageMetaData.Markup);

            imageMetaData.AuthorParsed = licenseInfo.AuthorName;
            imageMetaData.DescriptionParsed = licenseInfo.Description;
            imageMetaData.AllRegisteredLicenses = licenseInfo.AllRegisteredLicenses;
            imageMetaData.Markup = licenseInfo.Markup;
            imageMetaData.MarkupDownloadDate = licenseInfo.MarkupDownloadDate;
            imageMetaData.Notifications = licenseInfo.Notifications;
        }
    }
}