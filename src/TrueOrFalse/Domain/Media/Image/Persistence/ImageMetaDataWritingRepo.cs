using System.Web;
using Microsoft.AspNetCore.Http;
using Seedworks.Lib.Persistence;
using TrueOrFalse;
using TrueOrFalse.Maintenance;
using ISession = NHibernate.ISession;

public class ImageMetaDataWritingRepo : IRegisterAsInstancePerLifetime
{
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly LoadImageMarkups _loadImageMarkups;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RepositoryDb<ImageMetaData> _repo;

    public ImageMetaDataWritingRepo(ISession session,
        QuestionReadingRepo questionReadingRepo,
        LoadImageMarkups loadImageMarkups,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor)
    {
        _questionReadingRepo = questionReadingRepo;
        _loadImageMarkups = loadImageMarkups;
        _categoryRepository = categoryRepository;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _repo = new RepositoryDb<ImageMetaData>(session); 
    }

    public void StoreWiki(
        int typeId, 
        ImageType imageType, 
        int userId, 
        WikiImageMeta wikiMetaData)
    {
        var imageMeta = _imageMetaDataReadingRepo.GetBy(typeId, imageType) ?? new ImageMetaData();

        imageMeta.Type = imageType;
        imageMeta.TypeId = typeId;
        imageMeta.ApiHost = wikiMetaData.ApiHost;
        imageMeta.Source = ImageSource.WikiMedia;
        imageMeta.SourceUrl = wikiMetaData.ImageOriginalUrl;
        imageMeta.ApiResult = wikiMetaData.JSonResult;
        imageMeta.UserId = userId;

        _loadImageMarkups.Run(imageMeta);

        if(imageMeta.Id > 0 )
            Update(imageMeta);
        else
            Create(imageMeta);
    }

    public void StoreUploaded(int typeId, int userId, ImageType imageType, string licenseGiverName)
    {
        var imageMeta = _imageMetaDataReadingRepo.GetBy(typeId, imageType);
        if (imageMeta == null)
        {
            Create(
                new ImageMetaData
                {
                    TypeId = typeId,
                    Type = imageType,
                    Source = ImageSource.User,
                    AuthorParsed = licenseGiverName,
                    UserId = userId
                }
            );
        }
        else
        {
            imageMeta.Source = ImageSource.User;
            imageMeta.UserId = userId;
            imageMeta.AuthorParsed = licenseGiverName;

            Update(imageMeta);
        }
    }

    public void Create(ImageMetaData imageMetaData)
    {
        if(_httpContextAccessor.HttpContext != null)
            imageMetaData.LicenseState = new ImageMaintenanceInfo(imageMetaData, _questionReadingRepo, _categoryRepository).LicenseState;

        _repo.Create(imageMetaData);
    }

    public void Update(ImageMetaData imageMetaData)
    {
        if (_httpContextAccessor.HttpContext != null)
            imageMetaData.LicenseState = new ImageMaintenanceInfo(imageMetaData, _questionReadingRepo, _categoryRepository).LicenseState;

        _repo.Update(imageMetaData);
    }
}