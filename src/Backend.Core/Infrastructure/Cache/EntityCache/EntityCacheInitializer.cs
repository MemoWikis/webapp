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

        Log.Information("{Elapsed}" + " - EntityCache Start" + _customMessage, _stopWatch.Elapsed);

        InitializeUsers();
        InitializePageRelations();
        InitializePages();
        InitializeQuestions();
        InitializeShareInfos();

        Log.Information("{Elapsed}" + " - EntityCache PutIntoCache" + _customMessage, _stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }

    private void InitializeUsers()
    {
        var allUsers = _userReadingRepo.GetAll();
        Log.Information("{Elapsed}" + " - EntityCache UsersLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Log.Information("{Elapsed}" + " - EntityCache UsersCached " + _customMessage, _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());
    }

    private void InitializePageRelations()
    {
        var allRelations = pageRelationRepo.GetAll();
        Log.Information("{Elapsed}" + " - EntityCache PageRelationsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var relations = PageRelationCache.ToPageRelationCache(allRelations).ToList();
        MemoCache.Add(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
        Log.Information("{Elapsed}" + " - EntityCache PageRelationsCached " + _customMessage, _stopWatch.Elapsed);
    }

    private void InitializePages()
    {
        var allPages = pageRepository.GetAllEager();
        Log.Information("{Elapsed}" + " - EntityCache PagesLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var allPageViews = pageViewRepo.GetAllEager();
        Log.Information("{Elapsed}" + " - EntityCache PageViewsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var allPageChanges = pageChangeRepo.GetAll();
        Log.Information("{Elapsed}" + " - EntityCache PageChangesLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var pages = PageCacheItem.ToCachePages(allPages, allPageViews, allPageChanges).ToList();
        Log.Information("{Elapsed}" + " - EntityCache PagesCached " + _customMessage, _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyPages, pages.ToConcurrentDictionary());
        EntityCache.AddViewsLast30DaysToPages(pageViewRepo, pages);
        Log.Information("{Elapsed}" + " - EntityCache PagesPutIntoForeverCache " + _customMessage, _stopWatch.Elapsed);
    }

    private void InitializeQuestions()
    {
        var allQuestionChanges = _questionChangeRepo.GetAll();

        var allQuestions = _questionReadingRepo.GetAllEager();
        Log.Information("{Elapsed}" + " - EntityCache QuestionsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var allQuestionViews = _questionViewRepository.GetAllEager();
        Log.Information("{Elapsed}" + " - EntityCache QuestionViewsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions, allQuestionViews, allQuestionChanges).ToList();
        Log.Information("{Elapsed}" + " - EntityCache QuestionsCached " + _customMessage, _stopWatch.Elapsed);
        Log.Information("{Elapsed}" + " - EntityCache LoadAllEntities " + _customMessage, _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        MemoCache.Add(EntityCache.CacheKeyPageQuestionsList, EntityCache.GetPageQuestionsListForCacheInitializer(questions));

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
        Log.Information("{Elapsed}" + " - EntityCache ShareInfos Loaded " + _customMessage, _stopWatch.Elapsed);

        var shareCacheItems = allShareInfos.Select(ShareCacheItem.ToCacheItem).ToList();
        MemoCache.Add(EntityCache.CacheKeyPageShares, shareCacheItems.ToConcurrentDictionary());
    }
}

