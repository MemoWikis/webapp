using System.Collections.Concurrent;
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
        InitializeExtendedUsers();
        InitializeSkills();

        Log.Information("EntityCache PutIntoCache" + _customMessage + "{Elapsed}", _stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }

    private void InitializeUsers()
    {
        var allUsers = _userReadingRepo.GetAll();
        Log.Information("EntityCache UsersLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Log.Information("EntityCache UsersCached " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());
    }

    private void InitializePageRelations()
    {
        var allRelations = pageRelationRepo.GetAll();
        Log.Information("EntityCache PageRelationsLoadedFromRepo " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var relations = PageRelationCache.ToPageRelationCache(allRelations).ToList();
        MemoCache.Add(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
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

        MemoCache.Add(EntityCache.CacheKeyPages, pages.ToConcurrentDictionary());
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
        Log.Information("EntityCache ShareInfos Loaded " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        var shareCacheItems = allShareInfos.Select(ShareCacheItem.ToCacheItem).ToList();
        MemoCache.Add(EntityCache.CacheKeyPageShares, shareCacheItems.ToConcurrentDictionary());
    }

    private void InitializeExtendedUsers()
    {
        // Initialize empty ExtendedUsers cache - will be populated on-demand
        var extendedUsers = new ConcurrentDictionary<int, ExtendedUserCacheItem>();
        Log.Information("EntityCache ExtendedUsers Initialized " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyExtendedUsers, extendedUsers);
    }

    private void InitializeSkills()
    {
        // Initialize empty Skills cache - will be populated on-demand
        var skills = new ConcurrentDictionary<int, ConcurrentDictionary<int, UserSkillCacheItem>>();
        Log.Information("EntityCache Skills Initialized " + _customMessage + "{Elapsed}", _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeySkills, skills);
    }
}

