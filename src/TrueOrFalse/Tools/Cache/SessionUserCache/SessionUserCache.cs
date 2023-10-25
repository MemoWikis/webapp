using Seedworks.Web.State;
using System.Collections.Concurrent;
using ConcurrentCollections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

public class SessionUserCache : IRegisterAsInstancePerLifetime
{
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    public const int ExpirationSpanInMinutes = 600;
    private ConcurrentHashSet<string> _cacheKeys = new();
    private const string SessionUserCacheItemPrefix = "SessionUserCacheItem_";
    private string GetCacheKey(int userId) => SessionUserCacheItemPrefix + userId;


    public SessionUserCache(CategoryValuationReadingRepo categoryValuationReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo)
    {
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
    }
    public List<SessionUserCacheItem> GetAllCacheItems() 
    {
        var allUserIds = _userReadingRepo.GetAllIds();
        return allUserIds.Select(uId => GetItem(uId))
          .ToList();
    }

    public SessionUserCacheItem GetUser(int userId) =>
      GetItem(userId);

    private readonly string _createItemLockKey = "2FB5BC59-9E90-4511-809A-BC67A6D35F7F";
    public SessionUserCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));
        if (cacheItem != null)
            return cacheItem;

        lock (_createItemLockKey)
        {
            //recheck if the cache item exists
            Log.Information("GetUserCacheItem: {userId}", userId);
            cacheItem = Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));
            return cacheItem ?? CreateSessionUserItemFromDatabase(userId);
        }
    }

    public bool IsQuestionInWishknowledge(int userId, int questionId)
    {
        var cacheItem = GetItem(userId);

        var hasQuestionValuation = cacheItem.QuestionValuations.ContainsKey(questionId);

        if (!hasQuestionValuation)
            return false;

        return cacheItem.QuestionValuations[questionId].IsInWishKnowledge;
    }

    public IList<QuestionValuationCacheItem> GetQuestionValuations(int userId) =>
      GetItem(userId)?.QuestionValuations.Values
          .ToList() ?? new List<QuestionValuationCacheItem>();

    public IList<CategoryValuation> GetCategoryValuations(int userId) =>
      GetItem(userId).CategoryValuations.Values.ToList();

    public void AddOrUpdate(QuestionValuationCacheItem questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        lock ("7187a2c9-a3a2-42ca-8202-f9cb8cb54137")
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id, questionValuation,
              (k, v) => questionValuation);
        }
    }

    public void AddOrUpdate(CategoryValuation categoryValuation)
    {
        var cacheItem = GetItem(categoryValuation.UserId);

        lock ("82f573db-40a7-43d9-9e68-6cd78b626e8d")
        {
            cacheItem.CategoryValuations.AddOrUpdate(categoryValuation.CategoryId, categoryValuation,
              (k, v) => categoryValuation);
        }
    }

    public void AddOrUpdate(User user)
    {
        var cacheItem = GetItem(user.Id);
        cacheItem.AssignValues(user);
    }

    /// <summary> Used for question delete </summary>
    public void RemoveQuestionForAllUsers(int questionId)
    {
        foreach (var userId in _userReadingRepo.GetAllIds())
        {
            RemoveQuestionValuationForUser(userId, questionId);
        }
    }

    public void RemoveQuestionValuationForUser(int userId, int questionId)
    {
        var cacheItem = GetItem(userId);
        cacheItem?.QuestionValuations.TryRemove(questionId, out _);
    }

    public List<SessionUserCacheItem> GetAllActiveCaches()
    {
        List<SessionUserCacheItem> userCacheItems = new List<SessionUserCacheItem>();

        foreach (var userCacheKey in _cacheKeys.Where(k => k.Contains(SessionUserCacheItemPrefix)))
        {
            var item = Cache.Get<SessionUserCacheItem>(userCacheKey);
            if ( item != null)
            {
                userCacheItems.Add(item); 
            }
        }

        return userCacheItems;
    }

    /// <summary> Used for category delete </summary>
    public void RemoveAllForCategory(int categoryId, 
        CategoryValuationWritingRepo categoryValuationWritingRepo)
    {
        categoryValuationWritingRepo.DeleteCategoryValuation(categoryId);
        foreach (var userCache in GetAllActiveCaches())
        {
            userCache.CategoryValuations.TryRemove(categoryId, out var result);
        }
    }

    public void Remove(User user) => Remove(user.Id);

    public void Remove(int userId)
    {
        var cacheKey = GetCacheKey(userId);
        var cacheItem = Cache.Get<SessionUserCacheItem>(cacheKey);

        if (cacheItem != null)
        {
            Cache.Remove(cacheKey);
            _cacheKeys.TryRemove(cacheKey);
        }
    }


    public  SessionUserCacheItem CreateSessionUserItemFromDatabase(int userId)
    {
        var user = _userReadingRepo.GetById(userId);

        if (user == null) return null;

        var cacheItem = SessionUserCacheItem.CreateCacheItem(user);

        cacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
            _categoryValuationReadingRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v)));

        Add_UserCacheItem_to_cache(cacheItem);

        var addedCacheItem = Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));

        addedCacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
            _questionValuationReadingRepo.GetByUserWithQuestion(userId)
                .Select(v =>
                    new KeyValuePair<int, QuestionValuationCacheItem>(v.Question.Id,
                        QuestionValuationCacheItem.ToCacheItem(v))));

        _cacheKeys.Add(GetCacheKey(userId));

        return addedCacheItem;
    }

    private void Add_UserCacheItem_to_cache(SessionUserCacheItem cacheItem)
    {
        Cache.Add(GetCacheKey(cacheItem.Id), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes),
            slidingExpiration: true);
    }
}