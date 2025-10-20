public class LearningSessionConfig
{
    public int PageId { get; set; }
    public int MaxQuestionCount { get; set; } = 0;

    public int CurrentUserId { get; set; } = 0;

    public bool IsInTestMode { get; set; }
    public QuestionOrder QuestionOrder { get; set; } = QuestionOrder.SortByEasiest;
    public bool AnswerHelp { get; set; }
    public RepetitionType Repetition { get; set; } = RepetitionType.None;

    public bool IsAnonymous() => CurrentUserId < 0;

    public bool InWishknowledge { get; set; } = true;
    public bool NotWishKnowledge { get; set; } = true;
    public bool CreatedByCurrentUser { get; set; } = true;
    public bool NotCreatedByCurrentUser { get; set; } = true;
    public bool PrivateQuestions { get; set; } = true;
    public bool PublicQuestions { get; set; } = true;

    public bool NotLearned { get; set; } = true;
    public bool NeedsLearning { get; set; } = true;
    public bool NeedsConsolidation { get; set; } = true;
    public bool Solid { get; set; } = true;


    private PageCacheItem? _page;

    public PageCacheItem GetPage() => 
        _page ??= EntityCache.GetPage(this.PageId) ?? throw new InvalidOperationException();
}