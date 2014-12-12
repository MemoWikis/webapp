using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse.Maintenance
{
    public class ParseMarkupFromDb : IRegisterAsInstancePerLifetime
    {
        private readonly ImageMetaDataRepository _imgRepo;

        public ParseMarkupFromDb(ImageMetaDataRepository imgRepo)
        {
            _imgRepo = imgRepo;
        }

        public void Run()
        {
            var allImages = _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Source == ImageSource.WikiMedia)
                .List<ImageMetaData>();

            foreach (var img in allImages)
            {
                var licenseInfo = ParseImageMarkup.Run(img.Markup);

                img.AuthorParsed = licenseInfo.AuthorName;
                img.DescriptionParsed = licenseInfo.Description;
                img.AllRegisteredLicenses = licenseInfo.AllRegisteredLicenses;

                _imgRepo.Update(img);
            }
        }
    }
}
