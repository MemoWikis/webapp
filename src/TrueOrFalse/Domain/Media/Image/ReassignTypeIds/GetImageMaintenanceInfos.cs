using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NHibernate;

namespace TrueOrFalse
{
    public class GetImageMaintenanceInfos : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetImageMaintenanceInfos(ISession session){
            _session = session;
        }

        public List<ImageMaintenanceInfo> Run()
        {
            var imageMetaDatas = _session.QueryOver<ImageMetaData>().List();
            var result = new List<ImageMaintenanceInfo>();

            var categoryImgBasePath = new CategoryImageSettings().BasePath;
            var questionImgBasePath = new QuestionImageSettings().BasePath;
            var setImgBasePath = new QuestionSetImageSettings().BasePath;

            foreach (var imageMetaData in imageMetaDatas)
            {
                result.Add(new ImageMaintenanceInfo
                {
                    ImageId = imageMetaData.Id,
                    MetaData = imageMetaData,
                    TypeId = imageMetaData.TypeId,
                    InCategoryFolder = File.Exists(HttpContext.Current.Server.MapPath(
                        categoryImgBasePath + imageMetaData.TypeId + ".jpg")),
                    InQuestionFolder = File.Exists(HttpContext.Current.Server.MapPath(
                        questionImgBasePath + imageMetaData.TypeId + ".jpg")),
                    InSetFolder = File.Exists(HttpContext.Current.Server.MapPath(
                        setImgBasePath + imageMetaData.TypeId + ".jpg")),
                });
            }

            return result;
        }
    }
}
