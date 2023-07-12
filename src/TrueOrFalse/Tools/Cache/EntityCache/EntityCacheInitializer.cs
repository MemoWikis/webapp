using System.Diagnostics;
using System.Linq;


public class EntityCacheInitializer : BaseCache, IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionRepo _questionRepo;
    private readonly UserRepo _userRepo;

    public EntityCacheInitializer(CategoryRepository categoryRepository, 
        QuestionRepo questionRepo,
        UserRepo userRepo)
    {
        _categoryRepository = categoryRepository;
        _questionRepo = questionRepo;
        _userRepo = userRepo;
    }
    public void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allUsers = _userRepo.GetAll();
        Logg.r().Information("EntityCache UsersLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Logg.r().Information("EntityCache UsersCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());

        var allCategories = _categoryRepository.GetAllEager();
        Logg.r().Information("EntityCache CategoriesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var categories = CategoryCacheItem.ToCacheCategories(allCategories).ToList();
        Logg.r().Information("EntityCache CategoriesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyCategories, GraphService.AddChildrenIdsToCategoryCacheData(categories.ToConcurrentDictionary()));

        var allQuestions = _questionRepo.GetAllEager();
        Logg.r().Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions).ToList();
        Logg.r().Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);


        Logg.r().Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(EntityCache.CacheKeyCategoryQuestionsList, EntityCache.GetCategoryQuestionsList(questions));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }
        Logg.r().Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }
}

