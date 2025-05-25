public record struct LearningSessionResult()
{
    public int Index { get; set; } = 0;
    public LearningSessionCreator.Step[] Steps { get; set; } = [];
    public LearningSessionCreator.Step? CurrentStep { get; set; } = null;
    public int ActiveQuestionCount { get; set; } = 0;
    public bool AnswerHelp { get; set; } = true;
    public bool IsInTestMode { get; set; } = false;
    public string? MessageKey { get; set; } = null;
}