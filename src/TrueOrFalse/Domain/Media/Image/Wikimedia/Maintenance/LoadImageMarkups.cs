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

        //$temp: wird dieses globale (Nach-)Laden des Markups überhaupt benötigt, 
        //wenn Daten gleich beim ersten Speichern mit abgerufen werden?
        public void UpdateAll()
        {
            var allImages = _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Source == ImageSource.WikiMedia)
                .List<ImageMetaData>();
            
            Update(allImages);
        }

        public void UpdateAllWithoutAuthorizedMainLicense(bool loadMarkupFromWikipedia = true)
        {
            var allImages = _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Source == ImageSource.WikiMedia)
                .List<ImageMetaData>();

            var imagesToUpdate = allImages.Where(x => x.MainLicenseInfo == null ||
                x.ManualEntriesFromJson().ManualImageEvaluation == ManualImageEvaluation.ImageNotEvaluated);

            Update(imagesToUpdate, loadMarkupFromWikipedia);
        }

        private void Update(IEnumerable<ImageMetaData> imageMetaDataList, bool loadMarkupFromWikipedia = true)
        {
            foreach (var imageMetaData in imageMetaDataList)
            {
                Run(imageMetaData, loadMarkupFromWikipedia);

                try
                {
                    Logg.r().Information("LoadingImageMarkup.Update: {ImageMetaDataId} {Type}", imageMetaData.Id, imageMetaData.Type);
                    _imgRepo.Update(imageMetaData);
                    _imgRepo.Flush();
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "LoadingImageMarkup.Update: Error when saving imageMetaData {@ImageId}", imageMetaData.Id);
                    throw;
                }
            }
        }
    }
}