﻿using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace TrueOrFalse
{
    public class GetImageMaintenanceInfos : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetImageMaintenanceInfos(ISession session){
            _session = session;
        }

        public List<ImageMaintenanceInfo> Run(ImageMetaDataSearchSpec searchSpec)
        {
            var sessionUiData = Sl.R<SessionUiData>();
            var imageMetaDatas = Sl.R<ImageMetaDataRepository>().GetBy(sessionUiData.ImageMetaDataSearchSpec);

            return imageMetaDatas.Select(imageMetaData => new ImageMaintenanceInfo(imageMetaData)).ToList();
        }

        public List<ImageMaintenanceInfo> Run()
        {
            var imageMetaDatas = _session.QueryOver<ImageMetaData>().List();

            return imageMetaDatas.Select(imageMetaData => new ImageMaintenanceInfo(imageMetaData)).ToList();
        }
    }
}
 