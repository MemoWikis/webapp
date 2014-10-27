﻿using NHibernate;
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
                Create(
                    new ImageMetaData
                    {
                        Type = imageType,
                        TypeId = typeId,
                        Source = ImageSource.WikiMedia,
                        ApiHost = wikiMetaData.ApiHost,
                        SourceUrl = wikiMetaData.ImageUrl,
                        ApiResult = wikiMetaData.JSonResult,
                        UserId = userId
                    }
                );
            }
            else
            {
                imageMeta.SourceUrl = wikiMetaData.ImageUrl;
                imageMeta.Source = ImageSource.WikiMedia;
                imageMeta.ApiResult = wikiMetaData.JSonResult;
                imageMeta.UserId = userId;

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
