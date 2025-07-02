public record struct LearningSessionResultStep()
{
    public int Index { get; set; } = 0;
    public Step[] Steps { get; set; } = [];
    public Step? CurrentStep { get; set; } = null;
    public int ActiveQuestionCount { get; set; } = 0;
    public bool AnswerHelp { get; set; } = true;
    public bool IsInTestMode { get; set; } = false;
    public string? MessageKey { get; set; } = null;
}

public record struct Step
{
    public int id { get; set; }
    public AnswerState state { get; set; }
    public int index { get; set; }
    public bool isLastStep { get; set; }
}