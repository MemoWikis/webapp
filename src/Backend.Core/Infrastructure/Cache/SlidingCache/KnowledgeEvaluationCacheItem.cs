using Newtonsoft.Json;

/// <summary>
/// Cache item representing a user's skill with evaluation data
/// </summary>
public class KnowledgeEvaluationCacheItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PageId { get; set; }
    public KnowledgeSummary KnowledgeSummary { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? LastUpdatedAt { get; set; }

    public KnowledgeEvaluationCacheItem()
    {
        KnowledgeSummary = new KnowledgeSummary();
    }

    public static KnowledgeEvaluationCacheItem FromUserSkill(UserSkill userSkill)
    {
        var knowledgeSummary = new KnowledgeSummary();

        // Try to deserialize the evaluation JSON
        if (!string.IsNullOrEmpty(userSkill.EvaluationJson))
        {
            try
            {
                knowledgeSummary = JsonConvert.DeserializeObject<KnowledgeSummary>(userSkill.EvaluationJson) ?? new KnowledgeSummary();
            }
            catch
            {
                // If deserialization fails, use default evaluation
                knowledgeSummary = new KnowledgeSummary();
            }
        }

        return new KnowledgeEvaluationCacheItem
        {
            Id = userSkill.Id,
            UserId = userSkill.UserId,
            PageId = userSkill.PageId,
            KnowledgeSummary = knowledgeSummary,
            DateCreated = userSkill.DateCreated,
            LastUpdatedAt = userSkill.LastUpdatedAt
        };
    }
}
