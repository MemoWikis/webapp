public class SetImageLicenseStatus
{
    public static void RunForAll()
    {
        var imageMetaDataRepo = Sl.Resolve<ImageMetaDataRepo>();
        var allImageMetaDatas = imageMetaDataRepo.GetAll();

        Logg.r().Information("SetImageLicenseStatus for a total of" + allImageMetaDatas.Count + " images");
        foreach (var imageMetaData in allImageMetaDatas)
        {
            //on update the status will be reevaluated
            imageMetaDataRepo.Update(imageMetaData);
            Logg.r().Information("SetImageLicenseStatus for imageMetadata id " + imageMetaData.Id);
        }
            
    }
}