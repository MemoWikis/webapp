using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class ImageMetaDataReadingRepo : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly RepositoryDb<ImageMetaData> _repo;

    public ImageMetaDataReadingRepo(ISession session)
    {
        _session = session;
        _repo = new RepositoryDb<ImageMetaData>(session);
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

    public ImageMetaData GetById(int id)
    {
        return _repo.GetById(id);
    }
}