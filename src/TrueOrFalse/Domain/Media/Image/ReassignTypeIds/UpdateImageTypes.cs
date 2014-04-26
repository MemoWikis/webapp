using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class UpdateImageTypes : IRegisterAsInstancePerLifetime
    {
        private readonly GetImageMaintenanceInfos _getImageInfos;
        private readonly ImageMetaDataRepository _imgDataRepo;

        public UpdateImageTypes(GetImageMaintenanceInfos getImageInfos, ImageMetaDataRepository imgDataRepo)
        {
            _getImageInfos = getImageInfos;
            _imgDataRepo = imgDataRepo;
        }

        public void Run()
        {
            foreach (var imgMaintenanceInfo in _getImageInfos.Run())
            {
                if (imgMaintenanceInfo.IsClear())
                {
                    imgMaintenanceInfo.MetaData.Type = imgMaintenanceInfo.GetImageType();
                    _imgDataRepo.Update(imgMaintenanceInfo.MetaData);
                }
            }
        }
    }
}
