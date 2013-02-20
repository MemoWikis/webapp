using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace TrueOrFalse
{
    public class ImageMap : ClassMap<ImageMetaData>
    {
        public ImageMap()
        {
            Id(x => x.Id);

            Map(x => x.Type);
            Map(x => x.TypeId);

            Map(x => x.UserId);
            
            Map(x => x.Source);
            Map(x => x.SourceUrl);

            Map(x => x.LicenceInfo);

            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
