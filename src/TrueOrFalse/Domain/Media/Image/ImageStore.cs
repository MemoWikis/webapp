using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using TrueOrFalse;

public class ImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiImageMetaLoader _metaLoader;
    private readonly ImageMetaDataWritingRepo _imgMetaDataWritingRepo;
    private readonly Logg _logg;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        ImageMetaDataWritingRepo imgMetaDataWritingRepo,
        Logg logg)
    {
        _metaLoader = metaLoader;
        _imgMetaDataWritingRepo = imgMetaDataWritingRepo;
        _logg = logg;
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
            SaveImageToFile.Run(stream, imageSettings, _logg);
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
                SaveImageToFile.Run(stream, imageSettings, _logg);
            }
        }
        else
        {
            using (var stream = tmpImage.RelocateImage(relocateUrl))
            {
                SaveImageToFile.Run(stream, imageSettings, _logg);
            }
        }


        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }

    public void RunUploaded<T>(IFormFile imageFile, int typeId, int userId, string licenseGiverName) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        if (imageFile.Length == 0)
            return;

        using var stream = imageFile.OpenReadStream();

        SaveImageToFile.Run(stream, imageSettings, _logg);

        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }
}