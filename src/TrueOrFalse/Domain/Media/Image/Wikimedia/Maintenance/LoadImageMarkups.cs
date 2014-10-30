using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibraltar.Agent.Windows;

namespace TrueOrFalse.Maintenance
{
    public class LoadImageMarkups : IRegisterAsInstancePerLifetime
    {
        private readonly ImageMetaDataRepository _imgRepo;
        private readonly WikiImageLicenseLoader _wikiImageLicenseLoader;

        public LoadImageMarkups(
            ImageMetaDataRepository imgRepo, 
            WikiImageLicenseLoader wikiImageLicenseLoader)
        {
            _imgRepo = imgRepo;
            _wikiImageLicenseLoader = wikiImageLicenseLoader;
        }

        public void Run()
        {
            var allImages = _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Source == ImageSource.WikiMedia)
                .List<ImageMetaData>();

            foreach (var img in allImages)
            {
                var fileName = img.SourceUrl.Split('/').Last();
                var licenseInfo = _wikiImageLicenseLoader.Run(fileName, img.ApiHost);

                img.Author = licenseInfo.AuthorName;
                img.Description = licenseInfo.Description;
                img.Markup = licenseInfo.Markup;

                _imgRepo.Update(img);
            }
        }

        public ImageMetaData GetImageMetaDataById(int imageId)
        {
            return _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Id == imageId)
                .List<ImageMetaData>()
                .First(); 
        }

        public void tempRunOnSingleImage()
        {
            
        }

    }
}