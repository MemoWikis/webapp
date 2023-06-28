using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Maintenance
{
    public class LoadImageMarkups : IRegisterAsInstancePerLifetime
    {
        private readonly ImageMetaDataRepo _imgRepo;
        private readonly WikiImageLicenseLoader _wikiImageLicenseLoader;

        public LoadImageMarkups(
            ImageMetaDataRepo imgRepo,
            WikiImageLicenseLoader wikiImageLicenseLoader)
        {
            _imgRepo = imgRepo;
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