using System.Collections.Concurrent;

public class ExtendedUserCacheItem : UserCacheItem
{
    public ConcurrentDictionary<int, PageValuation> PageValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();
    public ConcurrentDictionary<int, AnswerRecord> AnswerCounter = new();
    private ConcurrentDictionary<int, KnowledgeEvaluationCacheItem> Skills = new();
    private ConcurrentDictionary<int, KnowledgeEvaluationCacheItem> KnowledgeSummaries = new();

    public void AddOrUpdateQuestionValuations(QuestionValuationCacheItem questionValuationCacheItem)
    {
        QuestionValuations.TryGetValue(questionValuationCacheItem.Question.Id, out var valuation);

        if (valuation == null)
        {
            var result = QuestionValuations
                .TryAdd(questionValuationCacheItem.Question.Id, questionValuationCacheItem);

            if (result == false)
                Log.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id}" +
                    $" could not be added in {nameof(AddOrUpdateQuestionValuations)}");
        }
        else
        {
            var result = QuestionValuations.TryUpdate(
                questionValuationCacheItem.Question.Id,
                questionValuationCacheItem,
                valuation);

            if (result == false)
                Log.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id} " +
                    $"could not be updated in {nameof(AddOrUpdateQuestionValuations)}");
        }

        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }

    public List<QuestionValuationCacheItem> GetAllQuestionValuations()
    {
        return QuestionValuations.Values.ToList();
    }

    public void AddOrUpdateSkill(KnowledgeEvaluationCacheItem knowledgeEvaluationCacheItem)
    {
        Skills.TryGetValue(knowledgeEvaluationCacheItem.PageId, out var existingSkill);

        if (existingSkill == null)
        {
            var result = Skills.TryAdd(knowledgeEvaluationCacheItem.PageId, knowledgeEvaluationCacheItem);

            if (result == false)
                Log.Error(
                    $"UserSkillCacheItem with PageId {knowledgeEvaluationCacheItem.PageId}" +
                    $" could not be added in {nameof(AddOrUpdateSkill)}");
        }
        else
        {
            var result = Skills.TryUpdate(
                knowledgeEvaluationCacheItem.PageId,
                knowledgeEvaluationCacheItem,
                existingSkill);

            if (result == false)
                Log.Error(
                    $"UserSkillCacheItem with PageId {knowledgeEvaluationCacheItem.PageId} " +
                    $"could not be updated in {nameof(AddOrUpdateSkill)}");
        }

        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }

    public void AddSkills(IList<KnowledgeEvaluationCacheItem> knowledgeEvaluationCacheItems)
    {
        foreach (var knowledgeEvaluationCacheItem in knowledgeEvaluationCacheItems)
        {
            AddOrUpdateSkill(knowledgeEvaluationCacheItem);
        }
    }

    public void RemoveSkill(int pageId)
    {
        Skills.TryRemove(pageId, out _);
        
        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }

    public KnowledgeEvaluationCacheItem? GetSkill(int pageId)
    {
        Skills.TryGetValue(pageId, out var skill);
        return skill;
    }

    public bool IsSkill(int pageId)
    {
        return Skills.ContainsKey(pageId);
    }

    public List<KnowledgeEvaluationCacheItem> GetAllSkills()
    {
        return Skills.Values.ToList();
    }

    public void PopulateSkills(IEnumerable<KnowledgeEvaluationCacheItem> skillsList)
    {
        Skills = new ConcurrentDictionary<int, KnowledgeEvaluationCacheItem>(
            skillsList.ToDictionary(skill => skill.PageId, skill => skill)
        );
        
        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }

    public void AddOrUpdateKnowledgeSummary(KnowledgeEvaluationCacheItem knowledgeEvaluationCacheItem)
    {
        KnowledgeSummaries.TryGetValue(knowledgeEvaluationCacheItem.PageId, out var existingKnowledgeSummary);

        if (existingKnowledgeSummary == null)
        {
            var result = KnowledgeSummaries.TryAdd(knowledgeEvaluationCacheItem.PageId, knowledgeEvaluationCacheItem);

            if (result == false)
                Log.Error(
                    $"KnowledgeSummaryCacheItem with PageId {knowledgeEvaluationCacheItem.PageId}" +
                    $" could not be added in {nameof(AddOrUpdateKnowledgeSummary)}");
        }
        else
        {
            var result = KnowledgeSummaries.TryUpdate(
                knowledgeEvaluationCacheItem.PageId,
                knowledgeEvaluationCacheItem,
                existingKnowledgeSummary);

            if (result == false)
                Log.Error(
                    $"KnowledgeSummaryCacheItem with PageId {knowledgeEvaluationCacheItem.PageId} " +
                    $"could not be updated in {nameof(AddOrUpdateKnowledgeSummary)}");
        }

        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }

    public void AddKnowledgeSummaries(IList<KnowledgeEvaluationCacheItem> knowledgeEvaluationCacheItems)
    {
        foreach (var knowledgeEvaluationCacheItem in knowledgeEvaluationCacheItems)
        {
            AddOrUpdateKnowledgeSummary(knowledgeEvaluationCacheItem);
        }
    }

    public void RemoveKnowledgeSummary(int pageId)
    {
        KnowledgeSummaries.TryRemove(pageId, out _);
        
        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }

    public KnowledgeEvaluationCacheItem? GetKnowledgeSummary(int pageId)
    {
        KnowledgeSummaries.TryGetValue(pageId, out var knowledgeSummary);
        return knowledgeSummary;
    }

    public List<KnowledgeEvaluationCacheItem> GetAllKnowledgeSummaries()
    {
        return KnowledgeSummaries.Values.ToList();
    }

    public void PopulateKnowledgeSummaries(IEnumerable<KnowledgeEvaluationCacheItem> knowledgeSummariesList)
    {
        KnowledgeSummaries = new ConcurrentDictionary<int, KnowledgeEvaluationCacheItem>(
            knowledgeSummariesList.ToDictionary(knowledgeSummary => knowledgeSummary.PageId, knowledgeSummary => knowledgeSummary)
        );
        
        // Update in sliding cache to refresh expiration and persist changes
        SlidingCache.AddOrUpdate(this);
    }
}