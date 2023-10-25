using System;

namespace AnswerBodyHelper;

public class SendAnswerToLearningSession
{
    public int Id { get; set; }
    public Guid QuestionViewGuid { get; set; }
    public string Answer { get; set; }
    public bool InTestMode { get; set; }

}

public class GetSolutionData
{
    public int Id { get; set; }
    public Guid QuestionViewGuid { get; set; }
    public int InteractionNumber { get; set; }
    public int MillisecondsSinceQuestionView { get; set; } = -1;
    public bool Unanswered { get; set; } = false;
}

public class MarkAsCorrectData
{
    public int Id { get; set; }
    public Guid QuestionViewGuid { get; set; }
    public int AmountOfTries { get; set; }
}