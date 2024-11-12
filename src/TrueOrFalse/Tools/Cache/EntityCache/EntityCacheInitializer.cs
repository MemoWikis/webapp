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

        var allPages = pageRepository.GetAllEager();
        Logg.r.Information("EntityCache PagesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allRelations = pageRelationRepo.GetAll();
        Logg.r.Information("EntityCache PageRelationsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var relations = PageRelationCache.ToPageRelationCache(allRelations).ToList();
        Cache.IntoForeverCache(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
        Logg.r.Information("EntityCache PageRelationsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allPageViews = pageViewRepo.GetAllEager();
        Logg.r.Information("EntityCache PageViewsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var allPageChanges = pageChangeRepo.GetAll();
        Logg.r.Information("EntityCache PageChangesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var pages = PageCacheItem.ToCachePages(allPages, allPageViews, allPageChanges).ToList();
        Logg.r.Information("EntityCache PagesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Cache.IntoForeverCache(EntityCache.CacheKeyPages, pages.ToConcurrentDictionary());
        EntityCache.AddViewsLast30DaysToPages(pageViewRepo, pages);
        Logg.r.Information("EntityCache PagesPutIntoForeverCache " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var allQuestionChanges = _questionChangeRepo.GetAll();

        var allQuestions = _questionReadingRepo.GetAllEager();
        Logg.r.Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allQuestionViews = _questionViewRepository.GetAllEager();
        Logg.r.Information("EntityCache QuestionViewsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions, allQuestionViews, allQuestionChanges).ToList();

        Logg.r.Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Logg.r.Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        Cache.IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());

        Cache.IntoForeverCache(EntityCache.CacheKeyPageQuestionsList, EntityCache.GetPageQuestionsListForCacheInitializer(questions));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }

        Logg.r.Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;

    }
}

