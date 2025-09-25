using Microsoft.AspNetCore.Http;

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
        Log.Information("ImageStore.RunWikimedia: Processing wikimedia image {ImageWikiFileName} for type {ImageType}, ID {TypeId}, user {UserId}", 
            imageWikiFileName, imageType, typeId, userId);
        
        var wikiMetaData = WikiImageMetaLoader.Run(imageWikiFileName, 1024);

        imageSettings.Init(typeId);
        Log.Information("ImageStore.RunWikimedia: About to delete old files for type {ImageType}, ID {TypeId}", imageType, typeId);
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

    public string RunPageContentUploadAndGetPath(IFormFile imageFile, int pageId, int userId, string licenseGiverName)
    {
        Log.Information("ImageStore.RunPageContentUploadAndGetPath: Uploading page content image for page {PageId}, user {UserId}, file {FileName} ({FileSize} bytes)", 
            pageId, userId, imageFile.FileName, imageFile.Length);
        
        var imageSettings = new ImageSettingsFactory(_httpContextAccessor, _questionReadingRepo)
            .Create<PageContentImageSettings>(pageId);

        imageSettings.Init(pageId);

        if (imageFile.Length == 0)
        {
            Log.Warning("ImageStore.RunPageContentUploadAndGetPath: Empty image file provided for page {PageId}", pageId);
            throw new Exception("imageFile is empty");
        }

        using var stream = imageFile.OpenReadStream();

        var path = SaveImageToFile.SaveContentImageAndGetPath(stream, imageSettings);
        
        Log.Information("ImageStore.RunPageContentUploadAndGetPath: Successfully uploaded page content image for page {PageId}, returning path {Path}", 
            pageId, path);
        
        return path;
    }

    public string RunQuestionContentUploadAndGetPath(IFormFile imageFile, int questionId, int userId,
        string licenseGiverName)
    {
        var imageSettings = new ImageSettingsFactory(_httpContextAccessor, _questionReadingRepo)
            .Create<QuestionContentImageSettings>(questionId);

        imageSettings.Init(questionId);

        if (imageFile.Length == 0)
            throw new Exception("imageFile is empty");

        using var stream = imageFile.OpenReadStream();

        var path = questionId > 0
            ? SaveImageToFile.SaveContentImageAndGetPath(stream, imageSettings)
            : SaveImageToFile.SaveTempQuestionContentImageAndGetPath(stream, imageSettings);
        return path;
    }
}