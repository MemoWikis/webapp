using System;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Maintenance;

namespace TrueOrFalse
{
    public class ImageMetaDataRepository : RepositoryDb<ImageMetaData> 
    {
        public ImageMetaDataRepository(ISession session) : base(session){}

        public ImageMetaData GetBy(int typeId, ImageType imageType)
        {
            return _session.QueryOver<ImageMetaData>()
                           .Where(x => x.TypeId == typeId)
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
            WikiImageMeta wikiMetaData)
        {
            var imageMeta = GetBy(typeId, imageType);
            if (imageMeta == null)
            {   
                var newImageMetaData = new ImageMetaData
                {
                    Type = imageType,
                    TypeId = typeId,
                    ApiHost = wikiMetaData.ApiHost,
                    Source = ImageSource.WikiMedia,
                    SourceUrl = wikiMetaData.ImageUrl,
                    ApiResult = wikiMetaData.JSonResult,
                    UserId = userId,
                };

                ServiceLocator.Resolve<LoadImageMarkups>().Run(newImageMetaData);
                newImageMetaData.AllRegisteredLicenses =
                    License.ToLicenseIdList(
                        LicenseParser.SortLicenses(LicenseParser.GetAllParsedLicenses(newImageMetaData.Markup)));
                LicenseParser.SetMainLicenseInfo(newImageMetaData);//$temp: ergänzen unter LoadImageMarkups?

                Create(newImageMetaData);
            }
            else
            {
                //$temp: warum hier kein ApiHost?
                imageMeta.Source = ImageSource.WikiMedia;
                imageMeta.SourceUrl = wikiMetaData.ImageUrl;
                imageMeta.ApiResult = wikiMetaData.JSonResult;
                imageMeta.UserId = userId;

                ServiceLocator.Resolve<LoadImageMarkups>().Run(imageMeta);
                imageMeta.AllRegisteredLicenses =
                    License.ToLicenseIdList(
                        LicenseParser.SortLicenses(LicenseParser.GetAllParsedLicenses(imageMeta.Markup)));
                UpdateMainLicenseInfo(imageMeta);

                Update(imageMeta);
                //$temp: beim Update werden nur die aufgeführten member überschrieben, oder?
            }
        }

        public static void UpdateMainLicenseInfo(ImageMetaData imageMetaData)
        {
            var mainLicenseInfo = MainLicenseInfo.FromJson(imageMetaData.MainLicenseInfo);
            var manuallyAddedAuthor = imageMetaData.ManualEntriesFromJson().AuthorManuallyAdded;
            if (mainLicenseInfo.MainLicenseId != 0)//Don't update if MainLicense already exists AND ...
            {
                if(mainLicenseInfo.MainLicenseId == License.FromLicenseIdList(imageMetaData.AllRegisteredLicenses)
                                                    .Where(license => license.LicenseApplicability == LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded)
                                                    .Select(license => license.Id)
                                                    .FirstOrDefault()//... current main license is most highly ranked of authorized parsed licenses OR...
                    || !String.IsNullOrEmpty(manuallyAddedAuthor))//... Author has been added manually
                return;
            } //If no MainLicense exists
                
            LicenseParser.SetMainLicenseInfo(imageMetaData);
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
