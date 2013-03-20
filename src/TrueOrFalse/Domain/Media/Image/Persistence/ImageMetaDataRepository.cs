using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class ImageMetaDataRepository : RepositoryDb<ImageMetaData> 
    {
        public ImageMetaDataRepository(ISession session) : base(session){}

        public ImageMetaData GetBy(int questionSetId, ImageType imageType)
        {
            return _session.QueryOver<ImageMetaData>()
                           .Where(x => x.TypeId == questionSetId)
                           .And(x => x.Type == imageType)
                           .SingleOrDefault<ImageMetaData>();
        }


        public void StoreSetWiki(int questionSetId, int userId, WikiImageMeta wikiMetaData){
            StoreWiki(questionSetId, userId, wikiMetaData, ImageType.QuestionSet);
        }
        
        public void StoreSetUploaded(int questionSetId, int userId, string licenceGiverName){
            StoreUploaded(questionSetId, userId, ImageType.QuestionSet, licenceGiverName);
        }

        private void StoreWiki(int typeId, int userId, WikiImageMeta wikiMetaData, ImageType imageType)
        {
            var imageMeta = GetBy(typeId, imageType);
            if (imageMeta == null)
            {
                Create(
                    new ImageMetaData
                    {
                        TypeId = typeId,
                        Type = imageType,
                        Source = ImageSource.WikiMedia,
                        SourceUrl = wikiMetaData.ImageUrl,
                        LicenceInfo = wikiMetaData.JSonResult,
                        UserId = userId
                    }
                );
            }
            else
            {
                imageMeta.SourceUrl = wikiMetaData.ImageUrl;
                imageMeta.Source = ImageSource.WikiMedia;
                imageMeta.LicenceInfo = wikiMetaData.JSonResult;
                imageMeta.UserId = userId;

                Update(imageMeta);
            }            
        }

        private void StoreUploaded(int typeId, int userId, ImageType imageType, string licenceGiverName)
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
                        LicenceInfo = licenceGiverName,
                        UserId = userId
                    }
                );
            }
            else
            {
                imageMeta.Source = ImageSource.User;
                imageMeta.UserId = userId;
                imageMeta.LicenceInfo = licenceGiverName;

                Update(imageMeta);
            }                        
        }
    }
}
