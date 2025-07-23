using Newtonsoft.Json;

/// <summary>
/// Cache item representing a user's skill with evaluation data
/// </summary>
public class UserSkillCacheItem
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public string PageName { get; set; } = "";
    public bool IsWiki { get; set; }
    public KnowledgeSummary Evaluation { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }

    public UserSkillCacheItem()
    {
        Evaluation = new KnowledgeSummary();
    }

    public static UserSkillCacheItem FromUserSkill(UserSkill userSkill, string pageName, bool isWiki)
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

        return new UserSkillCacheItem
        {
            Id = userSkill.Id,
            PageId = userSkill.PageId,
            PageName = pageName,
            IsWiki = isWiki,
            Evaluation = knowledgeSummary,
            AddedAt = userSkill.AddedAt,
            LastUpdatedAt = userSkill.LastUpdatedAt
        };
    }

    public UserSkill ToUserSkill(int userId)
    {
        return new UserSkill
        {
            Id = Id,
            UserId = userId,
            PageId = PageId,
            EvaluationJson = JsonConvert.SerializeObject(Evaluation),
            AddedAt = AddedAt,
            LastUpdatedAt = LastUpdatedAt
        };
    }
}
