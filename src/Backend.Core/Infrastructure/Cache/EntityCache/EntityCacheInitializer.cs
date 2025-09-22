﻿using System.Diagnostics;

public class EntityCacheInitializer(
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    PageRelationRepo pageRelationRepo,
    PageViewRepo pageViewRepo,
    QuestionViewRepository _questionViewRepository,
    PageChangeRepo pageChangeRepo,
    QuestionChangeRepo _questionChangeRepo,
    SharesRepository _sharesRepository,
    PageViewMmapCache _pageViewMmapCache,
    QuestionViewMmapCache _questionViewMmapCache,
    MmapCacheRefreshService _mmapCacheRefreshService) : IRegisterAsInstancePerLifetime
{
    private Stopwatch _stopWatch = null!;
    private string _customMessage = "";

    public void Init(string customMessage = "")
    {
        _stopWatch = Stopwatch.StartNew();
        _customMessage = customMessage;

        Log.Information("{Elapsed} - EntityCache Start{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        InitializeUsers();
        InitializePageRelations();
        InitializePages();
        InitializeQuestions();
        InitializeShareInfos();

        Log.Information("{Elapsed} - EntityCache PutIntoCache{CustomMessage}", _stopWatch.Elapsed, _customMessage);
        EntityCache.IsFirstStart = false;

        // Start background loading of today's views after cache initialization
        _mmapCacheRefreshService.LoadTodaysViewsInBackground();
    }

    private void InitializeUsers()
    {
        var allUsers = _userReadingRepo.GetAll();
        Log.Information("{Elapsed} - EntityCache UsersLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Log.Information("{Elapsed} - EntityCache UsersCached{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        MemoCache.Add(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());
    }

    private void InitializePageRelations()
    {
        var allRelations = pageRelationRepo.GetAll();
        Log.Information("{Elapsed} - EntityCache PageRelationsLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        var relations = PageRelationCache.ToPageRelationCache(allRelations).ToList();
        MemoCache.Add(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
        Log.Information("{Elapsed} - EntityCache PageRelationsCached{CustomMessage}", _stopWatch.Elapsed, _customMessage);
    }

    private void InitializePages()
    {
        var allPages = pageRepository.GetAllEager();
        Log.Information("{Elapsed} - EntityCache PagesLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        var allPageViews = LoadPageViewsFromMmapOrDatabase();

        var allPageChanges = pageChangeRepo.GetAll();
        Log.Information("{Elapsed} - EntityCache PageChangesLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        var pages = PageCacheItem.ToCachePages(allPages, allPageViews, allPageChanges).ToList();
        Log.Information("{Elapsed} - EntityCache PagesCached{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        MemoCache.Add(EntityCache.CacheKeyPages, pages.ToConcurrentDictionary());
        EntityCache.AddViewsLast30DaysToPages(pageViewRepo, pages);
        Log.Information("{Elapsed} - EntityCache PagesPutIntoForeverCache{CustomMessage}", _stopWatch.Elapsed, _customMessage);
    }

    private List<PageViewSummaryWithId> LoadPageViewsFromMmapOrDatabase()
    {
        var cachedPageViews = _pageViewMmapCache.LoadPageViews();
        return cachedPageViews.Any() 
            ? LoadPageViewsFromMmapCache(cachedPageViews)
            : LoadPageViewsFromDatabaseAndPersistToMmapCache();
    }

    private List<PageViewSummaryWithId> LoadPageViewsFromMmapCache(IEnumerable<PageViewSummaryWithId> cachedPageViews)
    {
        var pageViews = cachedPageViews.Select(view => new PageViewSummaryWithId(
            view.Count, view.DateOnly, view.PageId, view.LastPageViewCreatedAt)).ToList();

        Log.Information("{Elapsed} - EntityCache PageViewsLoadedFromMmap ({Count} entries){CustomMessage}",
            _stopWatch.Elapsed, pageViews.Count, _customMessage);

        // Return cached data immediately - today's views will be loaded in background
        return pageViews;
    }

    private List<PageViewSummaryWithId> LoadPageViewsFromDatabaseAndPersistToMmapCache()
    {
        // Load all page views from database (this is the fallback when cache is empty)
        var dbPageViews = pageViewRepo.GetAllEager();
        Log.Information("{Elapsed} - EntityCache PageViewsLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        _pageViewMmapCache.SaveAllPageViews(dbPageViews);
        return dbPageViews.ToList();
    }

    private void InitializeQuestions()
    {
        var allQuestionChanges = _questionChangeRepo.GetAll();

        var allQuestions = _questionReadingRepo.GetAllEager();
        Log.Information("{Elapsed} - EntityCache QuestionsLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        var allQuestionViews = LoadQuestionViewsFromMmapOrDatabase();

        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions, allQuestionViews, allQuestionChanges).ToList();
        Log.Information("{Elapsed} - EntityCache QuestionsCached{CustomMessage}", _stopWatch.Elapsed, _customMessage);
        Log.Information("{Elapsed} - EntityCache LoadAllEntities{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        MemoCache.Add(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        MemoCache.Add(EntityCache.CacheKeyPageQuestionsList, EntityCache.GetPageQuestionsListForCacheInitializer(questions));

        InitializeQuestionReferences(allQuestions);
    }

    private List<QuestionViewSummaryWithId> LoadQuestionViewsFromMmapOrDatabase()
    {
        var cachedQuestionViews = _questionViewMmapCache.LoadQuestionViews();
        return cachedQuestionViews.Any() 
            ? LoadQuestionViewsFromMmapCache(cachedQuestionViews)
            : LoadQuestionViewsFromDatabaseAndPersistToMmapCache();
    }

    private List<QuestionViewSummaryWithId> LoadQuestionViewsFromMmapCache(IEnumerable<QuestionViewSummaryWithId> cachedQuestionViews)
    {
        var questionViews = cachedQuestionViews.ToList();

        Log.Information("{Elapsed} - EntityCache QuestionViewsLoadedFromMmap ({Count} entries){CustomMessage}",
            _stopWatch.Elapsed, questionViews.Count, _customMessage);

        // Return cached data immediately - today's views will be loaded in background
        return questionViews;
    }

    private List<QuestionViewSummaryWithId> LoadQuestionViewsFromDatabaseAndPersistToMmapCache()
    {
        // Load all question views from database (this is the fallback when cache is empty)
        var dbQuestionViews = _questionViewRepository.GetAllEager();
        Log.Information("{Elapsed} - EntityCache QuestionViewsLoadedFromRepo{CustomMessage}", _stopWatch.Elapsed, _customMessage);

        _questionViewMmapCache.SaveAllQuestionViews(dbQuestionViews);
        return dbQuestionViews.ToList();
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

