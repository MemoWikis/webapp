﻿using System.Collections.Concurrent;

public class ExtendedUserCache(
    PageValuationReadingRepository pageValuationReadingRepository,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    AnswerRepo _answerRepo,
    UserSkillService _userSkillService)
    : IRegisterAsInstancePerLifetime
{
    // Note: No longer using session-based expiration

    public List<ExtendedUserCacheItem?> GetAllCacheItems()
    {
        return EntityCache.GetAllUsers()
            .Select(user => GetItem(user.Id))
            .Where(sessionUserCacheItem => sessionUserCacheItem != null)
            .ToList();
    }

    public ExtendedUserCacheItem GetUser(int userId)
    {
        if (userId < 1)
        {
            Log.Error($"userid < 1, {Environment.StackTrace} ");
            throw new Exception("userId does not exist");
        }

        var extendedUser = GetItem(userId);
        if (extendedUser != null)
            return extendedUser;

        // If user doesn't exist in extended cache, create and add them
        return Add(userId);
    }

    public bool ItemExists(int userId)
    {
        return EntityCache.GetExtendedUserByIdNullable(userId) != null;
    }

    public bool IsQuestionInWishknowledge(int userId, int questionId)
    {
        var cacheItem = GetItem(userId);

        if (cacheItem == null)
            return false;

        var hasQuestionValuation = cacheItem.QuestionValuations.ContainsKey(questionId);

        if (!hasQuestionValuation)
            return false;

        return cacheItem.QuestionValuations[questionId].IsInWishKnowledge;
    }

    public IList<QuestionValuationCacheItem> GetQuestionValuations(int userId) =>
        GetItem(userId)?.QuestionValuations.Values
            .ToList() ?? new List<QuestionValuationCacheItem>();

    public IList<PageValuation> GetPageValuations(int userId)
    {
        var item = GetItem(userId);

        if (item != null)
            return item.PageValuations.Values.ToList();

        Log.Error("sessionUserItem is null {userId}", userId);

        return new List<PageValuation>();
    }

    public ExtendedUserCacheItem? GetItem(int userId)
    {
        return EntityCache.GetExtendedUserByIdNullable(userId);
    }

    public void AddOrUpdate(QuestionValuationCacheItem questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        if (cacheItem != null)
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id,
                questionValuation,
                (k, v) => questionValuation);
        }
    }

    public void Update(User user)
    {
        var cacheItem = GetItem(user.Id);
        if (cacheItem != null)
            cacheItem.Populate(user);

        if (cacheItem == null)
            Log.Error($"should not be null {user.Id}");
    }

    public void Remove(User user) => Remove(user.Id);

    public void Remove(int userId)
    {
        var cacheItem = EntityCache.GetExtendedUserByIdNullable(userId);

        if (cacheItem != null)
            EntityCache.Remove(cacheItem);
    }

    public ExtendedUserCacheItem Add(int userId, PageViewRepo? _pageViewRepo = null)
    {
        lock ("2ba84bee-5294-420b-bd43-1decaa0d2d3e" + userId)
        {
            var sessionUserCacheItem = GetItem(userId);

            if (sessionUserCacheItem != null)
            {
                sessionUserCacheItem.PopulateSharedPages();

                if (sessionUserCacheItem.RecentPages == null && _pageViewRepo != null)
                    PopulateRecentPages(sessionUserCacheItem, _pageViewRepo);

                return sessionUserCacheItem;
            }

            var cacheItem = CreateExtendedUserCacheItem(userId, _pageViewRepo);
            cacheItem.PopulateSharedPages();

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
        return EntityCache.GetAllExtendedUsers().ToList();
    }

    /// <summary> Used for page delete </summary>
    public void RemoveAllForPage(
        int pageId,
        PageValuationWritingRepo pageValuationWritingRepo)
    {
        pageValuationWritingRepo.DeletePageValuation(pageId);
        foreach (var userCache in GetAllActiveCaches())
        {
            userCache.PageValuations.TryRemove(pageId, out var result);
        }
    }

    public ExtendedUserCacheItem CreateCacheItem(UserCacheItem userCacheItem)
    {
        var sessionUserCacheItem = new ExtendedUserCacheItem();
        sessionUserCacheItem.Populate(userCacheItem);

        return sessionUserCacheItem;
    }

    public ExtendedUserCacheItem CreateExtendedUserCacheItem(int userId, PageViewRepo? _pageViewRepo)
    {
        var cacheItem = CreateCacheItem(EntityCache.GetUserById(userId));

        PopulatePageValuations(cacheItem);
        PopulateQuestionValuations(cacheItem);
        PopulateAnswers(cacheItem);
        PopulateUserSkills(cacheItem);
        if (_pageViewRepo != null)
            PopulateRecentPages(cacheItem, _pageViewRepo);

        return cacheItem;
    }

    private void PopulatePageValuations(ExtendedUserCacheItem cacheItem)
    {
        cacheItem.PageValuations = new ConcurrentDictionary<int, PageValuation>(
            pageValuationReadingRepository
                .GetByUser(cacheItem.Id, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, PageValuation>(v.PageId, v))
        );
    }

    private void PopulateQuestionValuations(ExtendedUserCacheItem cacheItem)
    {
        cacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
            _questionValuationReadingRepo
                .GetByUserWithQuestion(cacheItem.Id)
                .Select(valuation =>
                    new KeyValuePair<int, QuestionValuationCacheItem>(
                        valuation.Question.Id,
                        QuestionValuationCacheItem.ToCacheItem(valuation)
                    )
                )
        );
    }

    private void PopulateAnswers(ExtendedUserCacheItem cacheItem)
    {
        var answers = _answerRepo.GetByUser(cacheItem.Id);
        if (answers != null)
        {
            cacheItem.AnswerCounter = new ConcurrentDictionary<int, AnswerRecord>(
                answers
                    .Where(a => a.Question != null)
                    .GroupBy(a => a.Question.Id)
                    .ToDictionary(g => g.Key, AnswerCache.AnswersToAnswerRecord)
            );
        }
    }

    private void PopulateUserSkills(ExtendedUserCacheItem cacheItem)
    {
        var userSkills = _userSkillService.GetUserSkills(cacheItem.Id);
        if (userSkills != null && userSkills.Any())
        {
            cacheItem.PopulateSkills(userSkills);
        }
    }

    private void PopulateRecentPages(ExtendedUserCacheItem cacheItem, PageViewRepo _pageViewRepo)
    {
        if (cacheItem.RecentPages == null || !cacheItem.RecentPages.PagesQueue.Any())
        {
            cacheItem.RecentPages = new RecentPages(cacheItem.Id, _pageViewRepo);
        }
    }

    private void AddToCache(ExtendedUserCacheItem cacheItem)
    {
        EntityCache.AddOrUpdate(cacheItem);
    }

    private void PopulateTokenUsage(ExtendedUserCacheItem cacheItem, AiUsageLogRepo _aiUsageLogRepo)
    {
        if (cacheItem.MonthlyTokenUsage == null)
        {
            cacheItem.MonthlyTokenUsage = new MonthlyTokenUsage(cacheItem.Id, _aiUsageLogRepo);
        }
    }
}