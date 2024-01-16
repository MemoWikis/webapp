using Seedworks.Web.State;
using System.Collections.Concurrent;
using ConcurrentCollections;

public class SessionUserCache : IRegisterAsInstancePerLifetime
{
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    public const int ExpirationSpanInMinutes = 600;
    private ConcurrentHashSet<string> _cacheKeys = new();
    private const string SessionUserCacheItemPrefix = "SessionUserCacheItem_";
    private string GetCacheKey(int userId) => SessionUserCacheItemPrefix + userId;
    private static object cacheLock = new object();

    public SessionUserCache(CategoryValuationReadingRepo categoryValuationReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo)
    {
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
    }
    public List<SessionUserCacheItem?> GetAllCacheItems()
    {
        var allUserIds = _userReadingRepo.GetAllIds();
        return allUserIds
            .Select(uId => GetItem(uId))
            .Where(suci => suci != null)
          .ToList();
    }

    public SessionUserCacheItem? GetUser(int userId) =>
      GetItem(userId);

    public SessionUserCacheItem? GetItem(int userId)
    {
        var user = Seedworks.Web.State.Cache.Get<SessionUserCacheItem>(GetCacheKey(userId)); 
        return user;
    }

    public bool ItemExists(int userId)
    {
        return Seedworks.Web.State.Cache.Contains(GetCacheKey(userId));
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

    public IList<CategoryValuation> GetCategoryValuations(int userId)
    {
        var item = GetItem(userId);

        if (item != null)
        {
            return item.CategoryValuations.Values.ToList();
        }

        Logg.r.Error("sessionUserItem is null");
        return new List<CategoryValuation>();
    }


    public void AddOrUpdate(QuestionValuationCacheItem questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        lock (cacheLock)
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id, questionValuation,
              (k, v) => questionValuation);
        }
    }

    public void AddOrUpdate(CategoryValuation categoryValuation)
    {
        var cacheItem = GetItem(categoryValuation.UserId);

        lock (cacheLock)
        {
            cacheItem.CategoryValuations.AddOrUpdate(categoryValuation.CategoryId, categoryValuation,
              (k, v) => categoryValuation);
        }
    }

    public void AddOrUpdate(User user)
    {
        var cacheItem = GetItem(user.Id);
        if (cacheItem == null)
        {
            cacheItem = CreateSessionUserItemFromDatabase(user.Id); 
            cacheItem.AssignValues(user);
            Add_UserCacheItem_to_cache(cacheItem);
        }
        Remove(user);
        cacheItem.AssignValues(user);
        Add_UserCacheItem_to_cache(cacheItem);
    }
    public void AddOrUpdate(SessionUserCacheItem user)
    {
        var cacheItem = GetItem(user.Id);
        if (cacheItem == null)
        {
            cacheItem = CreateSessionUserItemFromDatabase(user.Id);
            Add_UserCacheItem_to_cache(cacheItem);
            return;
        }
        Remove(user.Id);
        Add_UserCacheItem_to_cache(cacheItem);
    }

    public void RemoveQuestionForAllUsers(int questionId)
    {
        foreach (var user in EntityCache.GetAllUsers())
            RemoveQuestionValuationForUser(user.Id, questionId);
    }

    public void RemoveQuestionValuationForUser(int userId, int questionId)
    {
        if (ItemExists(userId))
        {
            var cacheItem = GetItem(userId);
            cacheItem?.QuestionValuations.TryRemove(questionId, out _);
        }
    }


    /// <summary>
    /// Get all active UserCaches
    /// </summary>
    /// <returns></returns>
    public List<SessionUserCacheItem> GetAllActiveCaches()
    {
        var allUsers = EntityCache.GetAllUsers();
        var userCacheItems = allUsers
            .Select(user => GetItem(user.Id))
            .Where(item => item != null)
            .ToList();

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
        var cacheItem = Seedworks.Web.State.Cache.Get<SessionUserCacheItem>(cacheKey);

        if (cacheItem != null)
        {
            Seedworks.Web.State.Cache.Remove(cacheKey);
            _cacheKeys.TryRemove(cacheKey);
        }
    }


    public SessionUserCacheItem CreateSessionUserItemFromDatabase(int userId)
    {
        var user = _userReadingRepo.GetById(userId);

        if (user == null) return null;

        var cacheItem = SessionUserCacheItem.CreateCacheItem(user);

        cacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
            _categoryValuationReadingRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v)));

        cacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
            _questionValuationReadingRepo.GetByUserWithQuestion(userId)
                .Select(v =>
                    new KeyValuePair<int, QuestionValuationCacheItem>(v.Question.Id,
                        QuestionValuationCacheItem.ToCacheItem(v))));

        return cacheItem;
    }

    private void Add_UserCacheItem_to_cache(SessionUserCacheItem cacheItem)
    {
        Seedworks.Web.State.Cache.Add(GetCacheKey(cacheItem.Id), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes),
            slidingExpiration: true);
    }
}