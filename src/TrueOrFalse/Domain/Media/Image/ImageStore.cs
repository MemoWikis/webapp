using System.Web;
using TrueOrFalse;

public class ImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiImageMetaLoader _metaLoader;
    private readonly ImageMetaDataWritingRepo _imgMetaDataWritingRepo;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        ImageMetaDataWritingRepo imgMetaDataWritingRepo)
    {
        _metaLoader = metaLoader;
        _imgMetaDataWritingRepo = imgMetaDataWritingRepo;
    }

    public void RunWikimedia(
        string imageWikiFileName,
        int typeId, 
        ImageType imageType,
        int userId, 
        IImageSettings imageSettings)
    {
        var wikiMetaData = _metaLoader.Run(imageWikiFileName, 1024);

        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        using (var stream = wikiMetaData.GetThumbImageStream())
        {
            //$temp: Bildbreite uebergeben und abhaengig davon versch. Groessen speichern?
            SaveImageToFile.Run(stream, imageSettings);
        }

        _imgMetaDataWritingRepo.StoreWiki(typeId, imageType, userId, wikiMetaData);
    }

    public void RunWikimedia<T>(
        string imageWikiFileName, 
        int typeId, 
        ImageType imageType,
        int userId) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();

        RunWikimedia(imageWikiFileName, typeId, imageType, userId, imageSettings);
    }

    public void RunUploaded<T>(TmpImage tmpImage, int typeId, int userId, string licenseGiverName, string relocateUrl = null) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        if (string.IsNullOrEmpty(relocateUrl))
        {
            using (var stream = tmpImage.GetStream())
            {
                SaveImageToFile.Run(stream, imageSettings);
            }
        }
        else
        {
            using (var stream = tmpImage.RelocateImage(relocateUrl))
            {
                SaveImageToFile.Run(stream, imageSettings);
            }
        }


        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }

    public void RunUploaded<T>(HttpPostedFileBase imagefile, int typeId, int userId, string licenseGiverName) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        SaveImageToFile.Run(imagefile.InputStream, imageSettings);

        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }
}