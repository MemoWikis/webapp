using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse;
using TrueOrFalse.Maintenance;

public class ImageMetaDataRepo : RepositoryDbBase<ImageMetaData>
{
    private readonly QuestionRepo _questionRepo;
    private readonly LoadImageMarkups _loadImageMarkups;

    public ImageMetaDataRepo(ISession session,
        QuestionRepo questionRepo, LoadImageMarkups loadImageMarkups) : base(session)
    {
        _questionRepo = questionRepo;
        _loadImageMarkups = loadImageMarkups;
    }

    public ImageMetaData GetBy(int typeId, ImageType imageType)
    {
        if (ImageMetaDataCache.IsInCache(typeId, imageType))
            return ImageMetaDataCache.FromCache(typeId, imageType);

        var metaData = GetBy(new List<int> {typeId}, imageType).FirstOrDefault();
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

        _loadImageMarkups.Run(imageMeta);

        if(imageMeta.Id > 0 )
            Update(imageMeta);
        else
            Create(imageMeta);
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
            imageMetaData.LicenseState = new ImageMaintenanceInfo(imageMetaData, _questionRepo).LicenseState;

        base.Create(imageMetaData);
    }

    public override void Update(ImageMetaData imageMetaData)
    {
        if (HttpContext.Current != null)
            imageMetaData.LicenseState = new ImageMaintenanceInfo(imageMetaData, _questionRepo).LicenseState;

        base.Update(imageMetaData);
    }
}