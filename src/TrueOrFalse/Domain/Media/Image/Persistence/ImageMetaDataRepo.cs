using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Criterion;
using SolrNet.Impl.FacetQuerySerializers;
using TrueOrFalse;
using TrueOrFalse.Maintenance;

public class ImageMetaDataRepo : RepositoryDbBase<ImageMetaData>
{
    public ImageMetaDataRepo(ISession session) : base(session){}

    public ImageMetaData GetBy(int typeId, ImageType imageType)
    {
        if (ImageMetaDataCache.IsInCache(typeId, imageType))
            return ImageMetaDataCache.FromCache(typeId, imageType);

        var metaData = GetBy(new List<int> {typeId}, imageType).FirstOrDefault();

        if (metaData == null && imageType == ImageType.QuestionSet)
        {
            var youtubeUrl = Sl.SetRepo.GetYoutbeUrl(typeId);
            if (!String.IsNullOrEmpty(youtubeUrl))
            {
                metaData = new ImageMetaData();
                metaData.TypeId = typeId;
                metaData.Type = imageType;
                metaData.IsYoutubePreviewImage = true;
                metaData.YoutubeKey = YoutubeVideo.GetVideoKeyFromUrl(youtubeUrl);
                metaData.MainLicenseInfo = new MainLicenseInfo
                {
                    MainLicenseId = 555,
                    Author = $"Youtube Vorschaubild: <a href='{youtubeUrl}'>gehe zu Youtube</a>"
                }.ToJson();
            }
        }

        return metaData;
    }

    public IList<ImageMetaData> GetBy(IList<int> typeIds, ImageType imageType, bool withNullValues = false)
    {
        if (withNullValues)
        {
            return _session.QueryOver<ImageMetaData>()
                .Where(Restrictions.In("TypeId", typeIds.ToList()))
                .List<ImageMetaData>();
        }

        return _session.QueryOver<ImageMetaData>()
            .Where(Restrictions.In("TypeId", typeIds.ToList()))
            .And(x => x.Type == imageType)
            .List<ImageMetaData>();
    }


    public IList<ImageMetaData> GetBy(ImageMetaDataSearchSpec searchSpec)
    {
        var query = _session.QueryOver<ImageMetaData>()
            .WhereRestrictionOn(x => x.LicenseState)
            .IsIn(searchSpec.LicenseStates);

        var result = query
            .Skip((searchSpec.CurrentPage - 1) * searchSpec.PageSize)
            .Take(searchSpec.PageSize)
            .List();

        searchSpec.TotalItems = query.RowCount();

        return result;
    }
        
    public void StoreWiki(
        int typeId, 
        ImageType imageType, 
        int userId, 
        WikiImageMeta wikiMetaData)
    {
        var imageMeta = GetBy(typeId, imageType) ?? new ImageMetaData();

        imageMeta.Type = imageType;
        imageMeta.TypeId = typeId;
        imageMeta.ApiHost = wikiMetaData.ApiHost;
        imageMeta.Source = ImageSource.WikiMedia;
        imageMeta.SourceUrl = wikiMetaData.ImageOriginalUrl;
        imageMeta.ApiResult = wikiMetaData.JSonResult;
        imageMeta.UserId = userId;

        ServiceLocator.Resolve<LoadImageMarkups>().Run(imageMeta);

        if(imageMeta.Id > 0 )
            Update(imageMeta);
        else
            Create(imageMeta);
    }

    public static void SetMainLicenseInfo(ImageMetaData imageMetaData, int MainLicenseId)
    {
        if (imageMetaData == null) return;
        if (LicenseImageRepo.GetAllAuthorizedLicenses().All(x => x.Id != MainLicenseId)) return;
        if (!LicenseParser.CheckLicenseRequirementsWithDb(LicenseImageRepo.GetById(MainLicenseId), imageMetaData).AllRequirementsMet) return;
        var manualEntries = imageMetaData.ManualEntriesFromJson();
        var mainLicenseInfo = new MainLicenseInfo
        {
            MainLicenseId = MainLicenseId,
            Author = !String.IsNullOrEmpty(manualEntries.AuthorManuallyAdded) ?
                manualEntries.AuthorManuallyAdded :
                imageMetaData.AuthorParsed,
            Markup = imageMetaData.Markup,
            MarkupDownloadDate = imageMetaData.MarkupDownloadDate,
        };
        imageMetaData.MainLicenseInfo = mainLicenseInfo.ToJson();
    }

    public void StoreUploaded(int typeId, int userId, ImageType imageType, string licenseGiverName)
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
                    AuthorParsed = licenseGiverName,
                    UserId = userId
                }
            );
        }
        else
        {
            imageMeta.Source = ImageSource.User;
            imageMeta.UserId = userId;
            imageMeta.AuthorParsed = licenseGiverName;

            Update(imageMeta);
        }
    }

    public override void Create(ImageMetaData imageMetaData)
    {
        if(HttpContext.Current != null)
            imageMetaData.LicenseState = new ImageMaintenanceInfo(imageMetaData).LicenseState;

        base.Create(imageMetaData);
    }

    public override void Update(ImageMetaData imageMetaData)
    {
        if (HttpContext.Current != null)
            imageMetaData.LicenseState = new ImageMaintenanceInfo(imageMetaData).LicenseState;

        base.Update(imageMetaData);
    }
}