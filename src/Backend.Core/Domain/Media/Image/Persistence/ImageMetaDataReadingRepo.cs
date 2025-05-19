using NHibernate;
using NHibernate.Criterion;


public class ImageMetaDataReadingRepo(ISession _session) : RepositoryDb<ImageMetaData>(_session)
{
    public ImageMetaData GetBy(int typeId, ImageType imageType)
    {
        if (ImageMetaDataCache.IsInCache(typeId, imageType, this))
        {
            var result = ImageMetaDataCache.FromCache(typeId, imageType, this);
            return result;
        }

        var metaData = GetBy(new List<int> { typeId }, imageType).FirstOrDefault();
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

    /// <summary>
    /// Retrieves all image metadata entries for the specified image type.
    /// </summary>
    /// <param name="imageType">The type of images to retrieve metadata for.</param>
    /// <returns>A dictionary of image metadata, with the ID as the key and the metadata object as the value.</returns>
    public IEnumerable<ImageMetaData> GetAll(ImageType imageType)
    {
        return _session.QueryOver<ImageMetaData>()
            .Where(x => x.Type == imageType)
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
}