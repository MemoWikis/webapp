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
    }
}
