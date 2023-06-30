using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace TrueOrFalse
{
    public class GetImageMaintenanceInfos : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly QuestionRepo _questionRepo;

        public GetImageMaintenanceInfos(ISession session, 
            QuestionRepo questionRepo)
        {
            _session = session;
            _questionRepo = questionRepo;
        }

        public List<ImageMaintenanceInfo> Run()
        {
            var imageMetaDatas = _session.QueryOver<ImageMetaData>().List();

            return imageMetaDatas.Select(imageMetaData => new ImageMaintenanceInfo(imageMetaData, _questionRepo)).ToList();
        }
    }
}
 