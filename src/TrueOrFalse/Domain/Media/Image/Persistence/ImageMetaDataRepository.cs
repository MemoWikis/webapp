using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class ImageMetaDataRepository : RepositoryDb<ImageMetaData> 
    {
        public ImageMetaDataRepository(ISession session) : base(session){}

        public ImageMetaData GetBy(int questionSetId, ImageType imageType)//$temp: "questionSetId" ist zu speziell, oder?
        {
            return _session.QueryOver<ImageMetaData>()
                           .Where(x => x.TypeId == questionSetId)
                           .And(x => x.Type == imageType)
                           .SingleOrDefault<ImageMetaData>();
        }
        
        public void StoreSetUploaded(int questionSetId, int userId, string licenseGiverName){
            StoreUploaded(questionSetId, userId, ImageType.QuestionSet, licenseGiverName);
        }

        public void StoreWiki(
            int typeId, 
            ImageType imageType, 
            int userId, 
            WikiImageMeta wikiMetaData,
            WikiImageLicenseInfo licenseInfo)
        {
            var imageMeta = GetBy(typeId, imageType);
            if (imageMeta == null)
            {   
                //$temp: Identisches hier oben?
                Create(
                    new ImageMetaData
                    {
                        Type = imageType,
                        TypeId = typeId,
                        ApiHost = wikiMetaData.ApiHost,
                        Source = ImageSource.WikiMedia,
                        SourceUrl = wikiMetaData.ImageUrl,
                        ApiResult = wikiMetaData.JSonResult,
                        UserId = userId,
                        //$temp neu (vorher nur unter LoadImageMarkups):
                        Author = licenseInfo.AuthorName,
                        Description = licenseInfo.Description,
                        Markup = licenseInfo.Markup,
                        //$temp Ganz neu:
                        MainLicense = LicenseParser.GetMainLicenseId(licenseInfo.Markup),
                        AllRegisteredLicenses = string.Join(", ", LicenseParser.GetAllLicenses(licenseInfo.Markup).Select(x => x.Id.ToString())),
                    }
                );
            }
            else
            {
                //$temp: warum hier kein ApiHost?
                imageMeta.Source = ImageSource.WikiMedia;
                imageMeta.SourceUrl = wikiMetaData.ImageUrl;
                imageMeta.ApiResult = wikiMetaData.JSonResult;
                imageMeta.UserId = userId;
                //$temp: neue von oben übernehmen oder übergreifend deklarieren

                Update(imageMeta);
            }            
        }

        private void StoreUploaded(int typeId, int userId, ImageType imageType, string licenseGiverName)
        {
            var imageMeta = GetBy(typeId, imageType);
            if (imageMeta == null)
            {
                Create(
                    new ImageMetaData
                    {
                        TypeId = typeId,
                        Type = imageType,
                        Source = ImageSource.User,
                        ApiResult = licenseGiverName,
                        UserId = userId
                    }
                );
            }
            else
            {
                imageMeta.Source = ImageSource.User;
                imageMeta.UserId = userId;
                imageMeta.ApiResult = licenseGiverName;

                Update(imageMeta);
            }                        
        }
    }
}
