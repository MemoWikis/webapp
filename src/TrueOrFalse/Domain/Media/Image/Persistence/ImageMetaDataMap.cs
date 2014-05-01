using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse
{
    public class ImageMetaDataMap : ClassMap<ImageMetaData>
    {
        public ImageMetaDataMap()
        {
            Id(x => x.Id);

            Map(x => x.Type);
            Map(x => x.TypeId);

            Map(x => x.UserId);
            
            Map(x => x.Source);
            Map(x => x.SourceUrl);

            Map(x => x.ApiResult).Length(Constants.VarCharMaxLength);

            Map(x => x.Author);
            Map(x => x.Description);
            Map(x => x.Markup);

            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
