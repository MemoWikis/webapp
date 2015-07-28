public class SetImageLicenseStatus
{
    public static void RunForAll()
    {
        var imageMetaDataRepo = Sl.Resolve<ImageMetaDataRepository>();
        var allImageMetaDatas = imageMetaDataRepo.GetAll();

        foreach (var imageMetaData in allImageMetaDatas)
            Run(imageMetaData);
    }

    public static void Run(ImageMetaData imageMetaData)
    {
        var imageInfo = new ImageMaintenanceInfo(imageMetaData);
        imageMetaData.LicenseState = imageInfo.LicenseState;

        Sl.Resolve<ImageMetaDataRepository>().Update(imageMetaData);
    }
}