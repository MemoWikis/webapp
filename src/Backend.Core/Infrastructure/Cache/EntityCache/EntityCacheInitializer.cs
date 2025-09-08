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
    SharesRepository _sharesRepository,
    PageViewMmapCache _pageViewMmapCache,
    QuestionViewMmapCache _questionViewMmapCache) : IRegisterAsInstancePerLifetime
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

        var allPageViews = LoadPageViewsFromMmapOrDatabase();

        var allPageChanges = pageChangeRepo.GetAll();
        Log.Information("{Elapsed}" + " - EntityCache PageChangesLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var pages = PageCacheItem.ToCachePages(allPages, allPageViews, allPageChanges).ToList();
        Log.Information("{Elapsed}" + " - EntityCache PagesCached " + _customMessage, _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyPages, pages.ToConcurrentDictionary());
        EntityCache.AddViewsLast30DaysToPages(pageViewRepo, pages);
        Log.Information("{Elapsed}" + " - EntityCache PagesPutIntoForeverCache " + _customMessage, _stopWatch.Elapsed);
    }

    private List<PageViewSummaryWithId> LoadPageViewsFromMmapOrDatabase()
    {
        // Load from mmap cache
        var (cachedPageViews, lastPageViewDate) = _pageViewMmapCache.LoadPageViews();
        var allPageViews = new List<PageViewSummaryWithId>();

        if (cachedPageViews.Any())
        {
            // Convert cached views to expected format
            allPageViews.AddRange(cachedPageViews.Select(view => new PageViewSummaryWithId(
                view.Count, view.DateOnly, view.PageId, view.DateCreated)));

            Log.Information("{Elapsed}" + " - EntityCache PageViewsLoadedFromMmap ({count} entries) " + _customMessage,
                _stopWatch.Elapsed, cachedPageViews.Count);

            // TODO: Load newer entries from database in background
            if (lastPageViewDate.HasValue)
            {
                Log.Information("Would load page views newer than {date} from database in background", lastPageViewDate.Value);
                // Background sync placeholder
            }
        }
        else
        {
            // First time - load all from database
            var dbPageViews = pageViewRepo.GetAllEager();
            allPageViews.AddRange(dbPageViews);
            Log.Information("{Elapsed}" + " - EntityCache PageViewsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

            // TODO: Save to mmap cache in background
            _pageViewMmapCache.SaveAllPageViews(dbPageViews);
        }

        return allPageViews;
    }

    private void InitializeQuestions()
    {
        var allQuestionChanges = _questionChangeRepo.GetAll();

        var allQuestions = _questionReadingRepo.GetAllEager();
        Log.Information("{Elapsed}" + " - EntityCache QuestionsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

        var allQuestionViews = LoadQuestionViewsFromMmapOrDatabase();

        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions, allQuestionViews, allQuestionChanges).ToList();
        Log.Information("{Elapsed}" + " - EntityCache QuestionsCached " + _customMessage, _stopWatch.Elapsed);
        Log.Information("{Elapsed}" + " - EntityCache LoadAllEntities " + _customMessage, _stopWatch.Elapsed);

        MemoCache.Add(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());
        MemoCache.Add(EntityCache.CacheKeyPageQuestionsList, EntityCache.GetPageQuestionsListForCacheInitializer(questions));

        InitializeQuestionReferences(allQuestions);
    }

    private List<QuestionViewSummaryWithId> LoadQuestionViewsFromMmapOrDatabase()
    {
        // Load from mmap cache
        var (cachedQuestionViews, lastQuestionViewDate) = _questionViewMmapCache.LoadQuestionViews();
        var allQuestionViews = new List<QuestionViewSummaryWithId>();
        
        if (cachedQuestionViews.Any())
        {
            // No conversion needed - same type
            allQuestionViews.AddRange(cachedQuestionViews);
            
            Log.Information("{Elapsed}" + " - EntityCache QuestionViewsLoadedFromMmap ({count} entries) " + _customMessage, 
                _stopWatch.Elapsed, cachedQuestionViews.Count);
            
            // TODO: Load newer entries from database in background
            if (lastQuestionViewDate.HasValue)
            {
                Log.Information("Would load question views newer than {date} from database in background", lastQuestionViewDate.Value);
                // Background sync placeholder
            }
        }
        else
        {
            // First time - load all from database
            var dbQuestionViews = _questionViewRepository.GetAllEager();
            allQuestionViews.AddRange(dbQuestionViews);
            Log.Information("{Elapsed}" + " - EntityCache QuestionViewsLoadedFromRepo " + _customMessage, _stopWatch.Elapsed);

            // TODO: Save to mmap cache in background
            _questionViewMmapCache.SaveAllQuestionViews(dbQuestionViews);
        }

        return allQuestionViews;
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

