using System.Diagnostics;

public class EntityCacheInitializer(
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    PageRelationRepo pageRelationRepo,
    PageViewRepo pageViewRepo,
    QuestionViewRepository _questionViewRepository,
    PageChangeRepo pageChangeRepo,
    QuestionChangeRepo _questionChangeRepo,
    AnswerRepo _answerRepo) : IRegisterAsInstancePerLifetime
{
    public void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r.Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var allUsers = _userReadingRepo.GetAll();
        Logg.r.Information("EntityCache UsersLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Logg.r.Information("EntityCache UsersCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        Cache.IntoForeverCache(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());

        var allCategories = pageRepository.GetAllEager();
        Logg.r.Information("EntityCache CategoriesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allRelations = pageRelationRepo.GetAll();
        Logg.r.Information("EntityCache CategoryRelationsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var relations = PageRelationCache.ToCategoryCacheRelations(allRelations).ToList();
        Cache.IntoForeverCache(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
        Logg.r.Information("EntityCache CategoryRelationsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allCategoryViews = pageViewRepo.GetAllEager();
        Logg.r.Information("EntityCache CategoryViewsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var allCategoryChanges = pageChangeRepo.GetAll();
        Logg.r.Information("EntityCache CategoryChangesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var categories = PageCacheItem.ToCacheCategories(allCategories, allCategoryViews, allCategoryChanges).ToList();
        Logg.r.Information("EntityCache CategoriesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Cache.IntoForeverCache(EntityCache.CacheKeyPages, categories.ToConcurrentDictionary());
        EntityCache.AddViewsLast30DaysToTopics(pageViewRepo, categories);
        Logg.r.Information("EntityCache CategoriesPutIntoForeverCache " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var allQuestionChanges = _questionChangeRepo.GetAll();

        var allQuestions = _questionReadingRepo.GetAllEager();
        Logg.r.Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allQuestionViews = _questionViewRepository.GetAllEager();
        Logg.r.Information("EntityCache QuestionViewsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions, allQuestionViews, allQuestionChanges).ToList();

        Logg.r.Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Logg.r.Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        Cache.IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());

        Cache.IntoForeverCache(EntityCache.CacheKeyCategoryQuestionsList, EntityCache.GetCategoryQuestionsListForCacheInitilizer(questions));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }

        Logg.r.Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;

    }
}

