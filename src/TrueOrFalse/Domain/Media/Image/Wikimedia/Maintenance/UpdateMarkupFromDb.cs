using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse.Maintenance
{
    public class UpdateMarkupFromDb : IRegisterAsInstancePerLifetime
    {
        private readonly ImageMetaDataRepository _imgRepo;

        public UpdateMarkupFromDb(ImageMetaDataRepository imgRepo)
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
                var licenceInfo = ParseImageMarkup.Run(img.Markup);

                img.Author = licenceInfo.AuthorName;
                img.Description = licenceInfo.Description;

                _imgRepo.Update(img);
            }
        }
    }
}
