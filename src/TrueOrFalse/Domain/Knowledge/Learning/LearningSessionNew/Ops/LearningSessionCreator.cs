using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class LearningSessionCreator
{
    public static LearningSession ForAnonymous(LearningSessionConfig config)
    {
        var questions = RandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId), config);

        return new LearningSession(questions.Select(q => new LearningSessionStep(q)).ToList(), config);
    }

    public static LearningSession ForLoggedInUser(LearningSessionConfig config)
    {
        List<Question> questions = new List<Question>();

        //if (config.AllQuestions && !UserCache.GetItem(config.CurrentUserId).IsFiltered)
        //    questions = OrderByProbability(RandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId),
        //        config)).ToList();
        //else if (config.IsNotQuestionInWishKnowledge && config.InWishknowledge && !config.CreatedByCurrentUser)
        //    questions = RandomLimited(IsInWuWiFromCategoryAndIsNotInWuwi(config.CurrentUserId, config.CategoryId), config);
        //else if (config.IsNotQuestionInWishKnowledge && config.CreatedByCurrentUser)
        //    questions = OrderByProbability(
        //            RandomLimited(NotWuwiFromCategoryOrIsAuthor(config.CurrentUserId, config.CategoryId), config))
        //        .ToList();
        //else if (config.IsNotQuestionInWishKnowledge)
        //    questions = OrderByProbability(
        //        RandomLimited(NotWuwiFromCategory(config.CurrentUserId, config.CategoryId), config)).ToList();
        //else if (config.InWishknowledge && config.CreatedByCurrentUser)
        //    questions = OrderByProbability(RandomLimited(
        //        WuwiQuestionsFromCategoryAndUserIsAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();
        //else if (config.InWishknowledge && !config.CreatedByCurrentUser)
        //    questions = OrderByProbability(
        //        RandomLimited(WuwiAndNotAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();
        //else if (config.InWishknowledge)
        //    questions = OrderByProbability(
        //        RandomLimited(WuwiQuestionsFromCategory(config.CurrentUserId, config.CategoryId).ToList(), config)).ToList();
        //else if (config.CreatedByCurrentUser)
        //    questions = OrderByProbability(
        //        RandomLimited(UserIsQuestionAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();

        return new LearningSession(questions.Distinct().Select(q => new LearningSessionStep(q)).ToList(), config);
    }

    public static int GetQuestionCount(LearningSessionConfig config)
    {
        config.MaxQuestionCount = 0;
        return BuildLearningSession(config.CategoryId).Steps.Count; 
    }

    private static List<Question> RandomLimited(List<Question> questions, LearningSessionConfig config)
    { 
        //if(config.MinProbability != 0 || config.MaxProbability != 100)
        //    questions = GetQuestionsFromMinToMaxProbability(config.MinProbability, config.MaxProbability, questions);

        questions.Shuffle();

        if (config.MaxQuestionCount == 0)
            return questions;

        if (config.MaxQuestionCount > questions.Count || config.MaxQuestionCount == -1)
            return questions;

        var amountQuestionsToDelete = questions.Count - config.MaxQuestionCount;
        questions.RemoveRange(0, amountQuestionsToDelete);
        
        return questions;
    }

    private static IList<Question> WuwiQuestionsFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation(userId), categoryId);
    }

    private static List<Question> WuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        return UserIsQuestionAuthor(userId, categoryId)
            .Concat(WuwiQuestionsFromCategory(userId, categoryId))
            .ToList();
    }


    private static List<Question> NotWuwiFromCategoryOrIsAuthor(int userId, int categoryId)
    {
        var isNotWuwi = NotWuwiFromCategory(userId, categoryId);
        return UserIsQuestionAuthor(userId, categoryId, true).Concat(isNotWuwi).ToList(); 
    }

    private static List<Question> WuwiAndNotAuthor(int userId, int categoryId)
    {
        var wuwi = WuwiQuestionsFromCategory(userId, categoryId);
        return wuwi.Where(q => new UserTinyModel(q.Creator).Id != userId).ToList();
    }

    private static List<Question> IsInWuWiFromCategoryAndIsNotInWuwi(int userId, int categoryId)
    {
        return GetCategoryQuestionsFromEntityCache(categoryId).Where(q =>
            new UserTinyModel(q.Creator).Id != userId)
            .ToList();
    }

    private static List<Question> NotWuwiFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation (userId), categoryId, true);
    }

    private static List<Question> UserIsQuestionAuthor(int userId, int categoryId, bool isNotInWuwi = false)
    {
        if (isNotInWuwi)
            return EntityCache.GetCategoryCacheItem(categoryId)
                .GetAggregatedQuestionsFromMemoryCache().Where(q =>
                    new UserTinyModel(q.Creator).Id == userId && !q.IsInWishknowledge())
                .ToList();

        if (UserCache.GetItem(Sl.CurrentUserId).IsFiltered)
            return EntityCache.GetCategoryCacheItem(categoryId)
                .GetAggregatedQuestionsFromMemoryCache().Where(q =>
                    new UserTinyModel(q.Creator).Id == userId && q.IsInWishknowledge())
                .ToList(); 

        return EntityCache.GetCategoryCacheItem(categoryId)
            .GetAggregatedQuestionsFromMemoryCache().Where(q =>
                 new UserTinyModel(q.Creator).Id  == userId)
            .ToList();
    }

    public static List<Question> GetCategoryQuestionsFromEntityCache(int categoryId)
    {
        return EntityCache.GetCategoryCacheItem(categoryId).GetAggregatedQuestionsFromMemoryCache().ToList();
    }

    private static IList<Question> OrderByProbability( List<Question> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
    }

    private static List<Question> CompareDictionaryWithQuestionsFromMemoryCache(Dictionary<int, int> dic1, int categoryId, bool isNotWuwi = false)
    {
        List<Question> questions = new List<Question>();
        var questionsFromEntityCache = EntityCache.GetCategoryCacheItem(categoryId)
            .GetAggregatedQuestionsFromMemoryCache().ToDictionary(q => q.Id);

        if (!isNotWuwi)
        {
            foreach (var q in questionsFromEntityCache)
            {
                if (dic1.ContainsKey(q.Key))
                    questions.Add(q.Value);
            }
        }
        else
        {
            foreach (var qId in dic1)
            {
                questionsFromEntityCache.Remove(qId.Key);
                questions = questionsFromEntityCache.Values.ToList(); 
            }
        }
        
        return questions;
    }

    private static Dictionary<int, int> GetIdsFromQuestionValuation(int userId)
    {
       return UserCache.GetQuestionValuations(userId).Where(qv => qv.IsInWishKnowledge)
            .Select(qv => qv.Question.Id).ToDictionary(q => q);
    }

    private static List<Question> GetQuestionsFromMinToMaxProbability(int minProbability, int maxProbability, List<Question> questions)
    {
        var result = questions.Where(q =>
            q.CorrectnessProbability >= minProbability && q.CorrectnessProbability <= maxProbability);
        return result.ToList(); 
    }

    public static LearningSession BuildLearningSession(int categoryId)
    {
        var config = new LearningSessionConfig
        {
            CategoryId = categoryId
        };
        var allQuestions = EntityCache.GetCategoryCacheItem(config.CategoryId).GetAggregatedQuestionsFromMemoryCache(onlyVisible: true);

        var filterDetails = new List<FilterDetail>();

        var filteredByWuwi = FilterByWuwi(config, allQuestions, filterDetails);
        var filteredByCreator = FilterByCreator(config, allQuestions, filterDetails);
        var filteredByVisibility = FilterByVisibility(config, allQuestions, filterDetails);
        var filteredByKnowledgeSummary = FilterByKnowledgeSummary(config, allQuestions, filterDetails);

        var listOfLists = new List<IEnumerable<int>>();

        listOfLists.Add(filteredByWuwi);
        listOfLists.Add(filteredByCreator);
        listOfLists.Add(filteredByVisibility);
        listOfLists.Add(filteredByKnowledgeSummary);

        var intersection = listOfLists
            .Skip(1)
            .Aggregate(
                new HashSet<int>(listOfLists.First()),
                (h, e) => { h.IntersectWith(e); return h; }
            );

        var filteredQuestions = allQuestions.Where(q => intersection.Any(i => i == q.Id)).ToList();

        var questions = filteredQuestions.Shuffle();
        questions = GetQuestionsByCount(config, questions);

        var learningSessionSteps = questions.Distinct().Select(q => new LearningSessionStep(q)).ToList();
        return new LearningSession(learningSessionSteps, config)
            {
                FilterDetails = filterDetails
            };
    }

    private static IList<Question> GetQuestionsByCount(LearningSessionConfig config, IList<Question> questions)
    {
        if (config.MaxQuestionCount > questions.Count || config.MaxQuestionCount == -1 || config.MaxQuestionCount == 0)
            return questions;

        return questions.Take(config.MaxQuestionCount).ToList();
    }
    private static List<int> FilterByWuwi(LearningSessionConfig config, IList<Question> allQuestions, List<FilterDetail> filteredDetails)
    {
        var inWuwi = WuwiQuestionsFromCategory(Sl.SessionUser.UserId, config.CategoryId);
        var inWuwiDetail = new FilterDetail
        {
            Ids = inWuwi.Select(q => q.Id),
            Count = inWuwi.Count,
            Key = "inWuwi",
            Group = "questionFilterOptions"
        };
        filteredDetails.Add(inWuwiDetail);

        var notInWuwi = NotWuwiFromCategory(Sl.SessionUser.UserId, config.CategoryId);
        var notInWuwiDetail = new FilterDetail
        {
            Ids = notInWuwi.Select(q => q.Id),
            Count = notInWuwi.Count,
            Key = "notInWuwi",
            Group = "questionFilterOptions"
        };
        filteredDetails.Add(notInWuwiDetail);

        if (config.InWuwi && !config.NotInWuwi)
            return inWuwiDetail.Ids.ToList();

        if (!config.InWuwi && config.NotInWuwi)
            return notInWuwiDetail.Ids.ToList();

        var filteredQuestionIds = new List<int>();
        filteredQuestionIds.AddRange(inWuwiDetail.Ids);
        filteredQuestionIds.AddRange(notInWuwiDetail.Ids);
        return filteredQuestionIds;
    }

    private static List<int> FilterByCreator(LearningSessionConfig config, IList<Question> questions, List<FilterDetail> filteredDetails)
    {
        var createdByCurrentUser = questions.Where(q => q.Creator.Id == Sl.SessionUser.User.Id);
        var createdByCurrentUserDetail = new FilterDetail
        {
            Ids = createdByCurrentUser.Select(q => q.Id),
            Count = createdByCurrentUser.Count(),
            Key = "createdByCurrentUser",
            Group = "questionFilterOptions"
        };
        filteredDetails.Add(createdByCurrentUserDetail);

        var notCreatedByCurrentUser = questions.Where(q => q.Creator.Id != Sl.SessionUser.User.Id);
        var notCreatedByCurrentUserDetail = new FilterDetail
        {
            Ids = notCreatedByCurrentUser.Select(q => q.Id),
            Count = notCreatedByCurrentUser.Count(),
            Key = "notCreatedByCurrentUser",
            Group = "questionFilterOptions"
        };
        filteredDetails.Add(notCreatedByCurrentUserDetail);

        if (config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser)
            return createdByCurrentUserDetail.Ids.ToList();

        if (!config.CreatedByCurrentUser && config.NotCreatedByCurrentUser)
            return notCreatedByCurrentUserDetail.Ids.ToList();

        var filteredQuestionIds = new List<int>();
        filteredQuestionIds.AddRange(createdByCurrentUserDetail.Ids);
        filteredQuestionIds.AddRange(notCreatedByCurrentUserDetail.Ids);
        return filteredQuestionIds;
    }

    private static List<int> FilterByVisibility(LearningSessionConfig config, IList<Question> questions, List<FilterDetail> filteredDetails)
    {
        var privateQuestions = questions.Where(q => q.Visibility == QuestionVisibility.Owner);
        var privateQuestionsDetail = new FilterDetail
        {
            Ids = privateQuestions.Select(q => q.Id),
            Count = privateQuestions.Count(),
            Key = "privateQuestions",
            Group = "questionFilterOptions"
        };
        filteredDetails.Add(privateQuestionsDetail);

        var publicQuestions = questions.Where(q => q.Visibility == QuestionVisibility.All);
        var publicQuestionsDetail = new FilterDetail
        {
            Ids = publicQuestions.Select(q => q.Id),
            Count = publicQuestions.Count(),
            Key = "publicQuestions",
            Group = "questionFilterOptions"
        };
        filteredDetails.Add(publicQuestionsDetail);

        if (config.PrivateQuestions && !config.PublicQuestions)
            return privateQuestionsDetail.Ids.ToList();

        if (!config.CreatedByCurrentUser && config.NotCreatedByCurrentUser)
            return publicQuestionsDetail.Ids.ToList();

        var filteredQuestionIds = new List<int>();
        filteredQuestionIds.AddRange(privateQuestionsDetail.Ids);
        filteredQuestionIds.AddRange(publicQuestionsDetail.Ids);
        return filteredQuestionIds;
    }

    private static List<int> FilterByKnowledgeSummary(LearningSessionConfig config, IList<Question> questions, List<FilterDetail> filteredDetails)
    {
        var allQuestionValuation = UserCache.GetQuestionValuations(Sl.SessionUser.UserId);

        var unansweredQuestions = questions.Where(q => allQuestionValuation.All(v => v.Id != q.Id));
        var notLearned = questions.Where(q =>
            unansweredQuestions.Any(v => v.Id == q.Id));
        var notLearnedDetail = new FilterDetail
        {
            Ids = notLearned.Select(q => q.Id),
            Count = notLearned.Count(),
            Key = "notLearned",
            Group = "knowledgeSummary"
        };
        filteredDetails.Add(notLearnedDetail);

        var needsLearningValuation = allQuestionValuation.Where(q =>
            q.CorrectnessProbability > 50 &&
            q.CorrectnessProbability < 50);
        var needsLearning = questions.Where(q =>
            needsLearningValuation.Any(v => v.Id == q.Id));
        var needsLearningDetail = new FilterDetail
        {
            Ids = needsLearning.Select(q => q.Id),
            Count = needsLearning.Count(),
            Key = "needsLearning",
            Group = "knowledgeSummary"
        };
        filteredDetails.Add(needsLearningDetail);

        var needsConsolidationValuation = allQuestionValuation.Where(q =>
            q.CorrectnessProbability >= 50 &&
            q.CorrectnessProbability < 80);
        var needsConsolidation = questions.Where(q =>
            needsConsolidationValuation.Any(v => v.Id == q.Id));
        var needsConsolidationDetail = new FilterDetail
        {
            Ids = needsConsolidation.Select(q => q.Id),
            Count = needsConsolidation.Count(),
            Key = "needsConsolidation",
            Group = "knowledgeSummary"
        };
        filteredDetails.Add(needsConsolidationDetail);

        var solidValuation = allQuestionValuation.Where(q =>
            q.CorrectnessProbability >= 80);
        var solid = questions.Where(q =>
            solidValuation.Any(v => v.Id == q.Id));
        var solidDetail = new FilterDetail
        {
            Ids = solid.Select(q => q.Id),
            Count = solid.Count(),
            Key = "solid",
            Group = "knowledgeSummary"
        };
        filteredDetails.Add(solidDetail);

        if (config.NotLearned && config.NeedsLearning && config.NeedsConsolidation && config.Solid ||
            !config.NotLearned && !config.NeedsLearning && !config.NeedsConsolidation && !config.Solid)
            return questions.GetIds().ToList();

        var filteredQuestionIds = new List<int>();

        if (config.NotLearned)
            filteredQuestionIds.AddRange(notLearnedDetail.Ids);

        if (config.NeedsLearning)
            filteredQuestionIds.AddRange(needsLearningDetail.Ids);

        if (config.NeedsConsolidation)
            filteredQuestionIds.AddRange(needsConsolidationDetail.Ids);

        if (config.Solid)
            filteredQuestionIds.AddRange(solidDetail.Ids);

        return filteredQuestionIds;
    }
}
public class FilterDetail
{
    public IEnumerable<int> Ids;
    public int Count;
    public string Key;
    public string Group;
}
