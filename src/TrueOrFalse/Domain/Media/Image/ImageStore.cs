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
        IImageSettings imageSettings;
        switch (imageMetaData.Type)
        {
            case ImageType.Category:
                imageSettings = new CategoryImageSettings(imageMetaData.TypeId); break;
            case ImageType.Question:
                imageSettings = new QuestionImageSettings(imageMetaData.TypeId); break;
            case ImageType.QuestionSet:
                imageSettings = new SetImageSettings(imageMetaData.TypeId); break;
            default:
                throw new Exception("invalid type");
        }

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

    public void RunUploaded<T>(TmpImage tmpImage, int typeId, int userId, string licenseGiverName) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        using (var stream = tmpImage.GetStream()){
            SaveImageToFile.Run(stream, imageSettings);
        }

        _imgMetaRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }

    public void Delete(ImageMetaData imageMetaData)
    {
        imageMetaData.GetSettings().DeleteFiles();
        _imgMetaRepo.Delete(imageMetaData.Id);
    }
}