using System.Collections.Concurrent;

public class ExtendedUserCacheItem : UserCacheItem
{
    public ConcurrentDictionary<int, PageValuation> PageValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();
    public ConcurrentDictionary<int, AnswerRecord> AnswerCounter = new();
    private ConcurrentDictionary<int, KnowledgeEvaluationCacheItem> Skills = new();
    private ConcurrentDictionary<int, KnowledgeSummary> KnowledgeSummaries = new();

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
    }

    public KnowledgeEvaluationCacheItem? GetSkill(int pageId)
    {
        Skills.TryGetValue(pageId, out var skill);
        return skill;
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
    }
}