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
    SharesRepository _sharesRepository) : IRegisterAsInstancePerLifetime
{
    private Stopwatch _stopWatch;
    private string _customMessage;

    public void Init(string customMessage = "")
    {
        _stopWatch = Stopwatch.StartNew();
        _customMessage = customMessage;

        Log.Information("EntityCache Start" + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        InitializeUsers();
        InitializePageRelations();
        InitializePages();
        InitializeQuestions();
        InitializeShareInfos();

        Log.Information("EntityCache PutIntoCache" + _customMessage + "{Elapsed}", _stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }

    private void InitializeUsers()
    {
        var allUsers = _userReadingRepo.GetAll();
        Log.Information("EntityCache UsersLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Log.Information("EntityCache UsersCached " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        MemoCache.IntoForeverCache(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());
    }

    private void InitializePageRelations()
    {
        var allRelations = pageRelationRepo.GetAll();
        Log.Information("EntityCache PageRelationsLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var relations = PageRelationCache.ToPageRelationCache(allRelations).ToList();
        MemoCache.IntoForeverCache(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
        Log.Information("EntityCache PageRelationsCached " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);
    }

    private void InitializePages()
    {
        var allPages = pageRepository.GetAllEager();
        Log.Information("EntityCache PagesLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var allPageViews = pageViewRepo.GetAllEager();
        Log.Information("EntityCache PageViewsLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var allPageChanges = pageChangeRepo.GetAll();
        Log.Information("EntityCache PageChangesLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var pages = PageCacheItem.ToCachePages(allPages, allPageViews, allPageChanges).ToList();
        Log.Information("EntityCache PagesCached " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        MemoCache.IntoForeverCache(EntityCache.CacheKeyPages, pages.ToConcurrentDictionary());
        EntityCache.AddViewsLast30DaysToPages(pageViewRepo, pages);
        Log.Information("EntityCache PagesPutIntoForeverCache " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);
    }

    private void InitializeQuestions()
    {
        var allQuestionChanges = _questionChangeRepo.GetAll();

        var allQuestions = _questionReadingRepo.GetAllEager();
        Log.Information("EntityCache QuestionsLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var allQuestionViews = _questionViewRepository.GetAllEager();
        Log.Information("EntityCache QuestionViewsLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions, allQuestionViews, allQuestionChanges).ToList();
        Log.Information("EntityCache QuestionsCached " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);
        Log.Information("EntityCache LoadAllEntities" + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        MemoCache.IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        MemoCache.IntoForeverCache(EntityCache.CacheKeyPageQuestionsList, EntityCache.GetPageQuestionsListForCacheInitializer(questions));

        InitializeQuestionReferences(allQuestions);
    }

    private void InitializeQuestionReferences(IEnumerable<Question> allQuestions)
    {
        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }
    }

    private void InitializeShareInfos()
    {
        var allShareInfos = _sharesRepository.GetAllEager();
        Log.Information("EntityCache ShareInfos Loaded " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var shareCacheItems = allShareInfos.Select(ShareCacheItem.ToCacheItem).ToList();
        MemoCache.IntoForeverCache(EntityCache.CacheKeyPageShares, shareCacheItems.ToConcurrentDictionary());
    }
}

