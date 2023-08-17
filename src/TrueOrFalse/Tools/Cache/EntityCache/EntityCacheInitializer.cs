using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


public class EntityCacheInitializer : BaseEntityCache, IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EntityCacheInitializer(CategoryRepository categoryRepository,
        UserReadingRepo userReadingRepo,
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _categoryRepository = categoryRepository;
        _userReadingRepo = userReadingRepo;
        _questionReadingRepo = questionReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    public void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allUsers = _userReadingRepo.GetAll();
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache UsersLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache UsersCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());

        var allCategories = _categoryRepository.GetAllEager();
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache CategoriesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var categories = CategoryCacheItem.ToCacheCategories(allCategories).ToList();
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache CategoriesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyCategories, GraphService.AddChildrenIdsToCategoryCacheData(categories.ToConcurrentDictionary()));

        var allQuestions = _questionReadingRepo.GetAllEager();
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions).ToList();
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);


        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(EntityCache.CacheKeyCategoryQuestionsList, EntityCache.GetCategoryQuestionsList(questions, _httpContextAccessor, _webHostEnvironment));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }
}

