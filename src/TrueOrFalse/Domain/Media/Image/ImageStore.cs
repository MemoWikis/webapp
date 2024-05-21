using Microsoft.AspNetCore.Http;
using TrueOrFalse;

public class ImageStore(
    ImageMetaDataWritingRepo _imgMetaDataWritingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : IRegisterAsInstancePerLifetime
{
    public void RunWikimedia(
        string imageWikiFileName,
        int typeId,
        ImageType imageType,
        int userId,
        IImageSettings imageSettings)
    {
        var wikiMetaData = WikiImageMetaLoader.Run(imageWikiFileName, 1024);

        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        using (var stream = wikiMetaData.GetThumbImageStream())
        {
            //$temp: Bildbreite uebergeben und abhaengig davon versch. Groessen speichern?
            SaveImageToFile.RemoveExistingAndSaveAllSizes(stream, imageSettings);
        }

        _imgMetaDataWritingRepo.StoreWiki(typeId, imageType, userId, wikiMetaData);
    }

    public void RunWikimedia<T>(
        string imageWikiFileName,
        int typeId,
        ImageType imageType,
        int userId) where T : IImageSettings
    {
        var imageSettings = new ImageSettingsFactory(_httpContextAccessor, _questionReadingRepo)
            .Create<T>(typeId);
        RunWikimedia(imageWikiFileName, typeId, imageType, userId, imageSettings);
    }

    public void RunUploaded<T>(
        TmpImage tmpImage,
        int typeId,
        int userId,
        string licenseGiverName,
        string relocateUrl = null) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        if (string.IsNullOrEmpty(relocateUrl))
        {
            using (var stream = tmpImage.GetStream())
            {
                SaveImageToFile.RemoveExistingAndSaveAllSizes(stream, imageSettings);
            }
        }
        else
        {
            using (var stream = tmpImage.RelocateImage(relocateUrl))
            {
                SaveImageToFile.RemoveExistingAndSaveAllSizes(stream, imageSettings);
            }
        }

        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType,
            licenseGiverName);
    }

    public void RunUploaded<T>(IFormFile imageFile, int typeId, int userId, string licenseGiverName)
        where T : IImageSettings
    {
        var imageSettings = new ImageSettingsFactory(_httpContextAccessor, _questionReadingRepo)
            .Create<T>(typeId);

        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        if (imageFile.Length == 0)
            return;

        using var stream = imageFile.OpenReadStream();

        SaveImageToFile.RemoveExistingAndSaveAllSizes(stream, imageSettings);

        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType,
            licenseGiverName);
    }
}