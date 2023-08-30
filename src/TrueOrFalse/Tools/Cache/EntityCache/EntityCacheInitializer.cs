using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;


public class EntityCacheInitializer : BaseEntityCache, IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger _logg;


    public EntityCacheInitializer(
         CategoryRepository categoryRepository,
        UserReadingRepo userReadingRepo,
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        Logg logg
    )
    {
        _categoryRepository = categoryRepository;
        _userReadingRepo = userReadingRepo;
        _questionReadingRepo = questionReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _logg = logg.r();
    }
    public void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        _logg.Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allUsers = _userReadingRepo.GetAll();
        _logg.Information("EntityCache UsersLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        _logg.Information("EntityCache UsersCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());

        var allCategories = _categoryRepository.GetAllEager();
        _logg.Information("EntityCache CategoriesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var categories = CategoryCacheItem.ToCacheCategories(allCategories, _logg).ToList();
        _logg.Information("EntityCache CategoriesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyCategories, GraphService.AddChildrenIdsToCategoryCacheData(categories.ToConcurrentDictionary()));

        var allQuestions = _questionReadingRepo.GetAllEager();
        _logg.Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions).ToList();
        _logg.Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        _logg.Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(EntityCache.CacheKeyCategoryQuestionsList, EntityCache.GetCategoryQuestionsList(questions, _httpContextAccessor, _webHostEnvironment));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }
        _logg.Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }
}

