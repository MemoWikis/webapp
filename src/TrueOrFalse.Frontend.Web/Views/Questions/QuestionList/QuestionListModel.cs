using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

public class QuestionListModel
{
    private SessionUser _sessionUser { get; }
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly ExtendedUserCache _extendedUserCache;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly QuestionReadingRepo _questionReadingRepo;
    public int CategoryId;

    public QuestionListModel(
        LearningSessionCache learningSessionCache,
        SessionUser sessionUser,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        ExtendedUserCache extendedUserCache,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        QuestionReadingRepo questionReadingRepo)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _extendedUserCache = extendedUserCache;
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _questionReadingRepo = questionReadingRepo;
    }
}