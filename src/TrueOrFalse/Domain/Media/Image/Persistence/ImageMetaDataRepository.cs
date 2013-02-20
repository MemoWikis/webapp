using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class ImageMetaDataRepository : RepositoryDb<ImageMetaData> 
    {
        public ImageMetaDataRepository(ISession session) : base(session)
        {
        }
    }
}
