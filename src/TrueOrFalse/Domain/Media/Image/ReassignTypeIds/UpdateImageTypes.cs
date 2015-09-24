using TrueOrFalse;

public class UpdateImageTypes : IRegisterAsInstancePerLifetime
{
    private readonly GetImageMaintenanceInfos _getImageInfos;
    private readonly ImageMetaDataRepo _imgDataRepo;

    public UpdateImageTypes(GetImageMaintenanceInfos getImageInfos, ImageMetaDataRepo imgDataRepo)
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

