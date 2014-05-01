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
        private readonly WikiImageLicenceLoader _wikiImageLicenceLoader;

        public LoadImageMarkups(
            ImageMetaDataRepository imgRepo, 
            WikiImageLicenceLoader wikiImageLicenceLoader)
        {
            _imgRepo = imgRepo;
            _wikiImageLicenceLoader = wikiImageLicenceLoader;
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
                var licenceInfo = _wikiImageLicenceLoader.Run(fileName);

                img.Author = licenceInfo.AuthorName;
                img.Description = licenceInfo.Description;
                img.Markup = licenceInfo.Markup;

                _imgRepo.Update(img);
            }
        }
    }
}