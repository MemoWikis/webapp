using System;
using TrueOrFalse;

public class ImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiImageMetaLoader _metaLoader;
    private readonly ImageMetaDataRepo _imgMetaRepo;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        ImageMetaDataRepo imgMetaRepo)
    {
        _metaLoader = metaLoader;
        _imgMetaRepo = imgMetaRepo;
    }

    public void ReloadWikipediaImage(ImageMetaData imageMetaData)
    {
        var imageSettings = ImageSettings.InitByType(imageMetaData);
        RunWikimedia(imageMetaData.SourceUrl, imageMetaData.TypeId, imageMetaData.Type, imageMetaData.UserId, imageSettings);
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

        _imgMetaRepo.StoreWiki(typeId, imageType, userId, wikiMetaData);
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


        _imgMetaRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }

    public void Delete(ImageMetaData imageMetaData)
    {
        imageMetaData.GetSettings().DeleteFiles();
        _imgMetaRepo.Delete(imageMetaData.Id);
    }
}