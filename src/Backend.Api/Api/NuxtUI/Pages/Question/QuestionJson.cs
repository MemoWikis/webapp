namespace QuestionListJson;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CorrectnessProbability { get; set; }
    public string LinkToQuestion { get; set; }
    public string ImageData { get; set; }
    public bool IsInWishKnowledge { get; set; }
    public bool HasPersonalAnswer { get; set; }
    public int LearningSessionStepCount { get; set; }
    public string LinkToComment { get; set; }
    public string LinkToQuestionVersions { get; set; }
    public int SessionIndex { get; set; }
    public QuestionVisibility Visibility { get; set; }
    public int CreatorId { get; set; } = 0;
    public KnowledgeStatus KnowledgeStatus { get; set; } = KnowledgeStatus.NotLearned;
}