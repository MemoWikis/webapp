using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using TrueOrFalse;

public class ImageStore : IRegisterAsInstancePerLifetime
{
    private readonly ImageMetaDataWritingRepo _imgMetaDataWritingRepo;
    private readonly Logg _logg;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageStore(
        ImageMetaDataWritingRepo imgMetaDataWritingRepo,
        Logg logg,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo,
        IWebHostEnvironment webHostEnvironment
        )
    {

        _imgMetaDataWritingRepo = imgMetaDataWritingRepo;
        _logg = logg;
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
        _webHostEnvironment = webHostEnvironment;
    }

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
        var imageSettings = new ImageSettingsFactory(_httpContextAccessor, _webHostEnvironment, _questionReadingRepo)
            .Create<T>(typeId);

        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        if (imageFile.Length == 0)
            return;

        using var stream = imageFile.OpenReadStream();

        SaveImageToFile.Run(stream, imageSettings, _logg);

        _imgMetaDataWritingRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }
}