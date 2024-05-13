using System.Collections.Concurrent;

public class ExtendedUserCache(
    UserReadingRepo _userReadingRepo,
    CategoryValuationReadingRepo _categoryValuationReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    public const int ExpirationSpanInMinutes = 600;
    private const string SessionUserCacheItemPrefix = "SessionUserCacheItem_";

    private string GetCacheKey(int userId) => SessionUserCacheItemPrefix + userId;

    public List<ExtendedUserCacheItem?> GetAllCacheItems()
    {
        return EntityCache.GetAllUsers()
            .Select(user => GetItem(user.Id))
            .Where(sessionUserCacheItem => sessionUserCacheItem != null)
            .ToList();
    }

    public ExtendedUserCacheItem GetUser(int userId)
    {
        var user = _userReadingRepo.GetById(userId);
        if (user == null)
        {
            Logg.r.Error("user should not be null here + GetUser()");
            throw new NullReferenceException();
        }

        return
            GetItem(userId) ?? Add(user);
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
            return item.CategoryValuations.Values.ToList();

        Logg.r.Error("sessionUserItem is null {userId}", userId);

        return new List<CategoryValuation>();
    }

    public ExtendedUserCacheItem? GetItem(int userId) =>
        Seedworks.Web.State.Cache.Get<ExtendedUserCacheItem>(GetCacheKey(userId));

    public void AddOrUpdate(QuestionValuationCacheItem questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id,
            questionValuation,
            (k, v) => questionValuation);
    }

    public void Update(User user)
    {
        var cacheItem = GetItem(user.Id);
        if (cacheItem == null)
            throw new NullReferenceException($"should not be null {user.Id}");

        cacheItem.Populate(user);
    }

    public void Remove(User user) => Remove(user.Id);

    public void Remove(int userId)
    {
        var cacheKey = GetCacheKey(userId);
        var cacheItem = Seedworks.Web.State.Cache.Get<ExtendedUserCacheItem>(cacheKey);

        if (cacheItem != null)
            Seedworks.Web.State.Cache.Remove(cacheKey);
    }

    public ExtendedUserCacheItem Add(User user)
    {
        lock ("2ba84bee-5294-420b-bd43-1decaa0d2d3e" + user.Id)
        {
            var sessionUserCacheItem = GetItem(user.Id);

            if (sessionUserCacheItem != null)
                return sessionUserCacheItem;

            var cacheItem = CreateSessionUserItemFromDatabase(user);

            cacheItem.Populate(user);
            AddToCache(cacheItem);
            return cacheItem;
        }
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
    public List<ExtendedUserCacheItem> GetAllActiveCaches()
    {
        var allUsers = EntityCache.GetAllUsers();
        var userCacheItems = allUsers
            .Select(user => GetItem(user.Id))
            .Where(item => item != null)
            .ToList();

        return userCacheItems;
    }

    /// <summary> Used for category delete </summary>
    public void RemoveAllForCategory(
        int categoryId,
        CategoryValuationWritingRepo categoryValuationWritingRepo)
    {
        categoryValuationWritingRepo.DeleteCategoryValuation(categoryId);
        foreach (var userCache in GetAllActiveCaches())
        {
            userCache.CategoryValuations.TryRemove(categoryId, out var result);
        }
    }

    public ExtendedUserCacheItem CreateSessionUserItemFromDatabase(User user)
    {
        var cacheItem = ExtendedUserCacheItem.CreateCacheItem(user);

        cacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
            _categoryValuationReadingRepo
                .GetByUser(user.Id, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v)));

        cacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
            _questionValuationReadingRepo
                .GetByUserWithQuestion(user.Id)
                .Select(valuation =>
                    new KeyValuePair<int, QuestionValuationCacheItem>(
                        valuation.Question.Id,
                        QuestionValuationCacheItem.ToCacheItem(valuation)
                    )
                )
        );

        return cacheItem;
    }

    private void AddToCache(ExtendedUserCacheItem cacheItem)
    {
        Seedworks.Web.State.Cache.Add(GetCacheKey(cacheItem.Id), cacheItem,
            TimeSpan.FromMinutes(ExpirationSpanInMinutes),
            slidingExpiration: true);
    }
}