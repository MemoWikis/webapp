public class AnswerQuestionResult
{
    public bool IsCorrect;

    /// <summary>
    /// The answer given by the user
    /// </summary>
    public string AnswerGiven = "";

    public string CorrectAnswer = "";

    public bool NewStepAdded;

    public int NumberSteps;

    public int? LearningSessionId;

    public string LearningSessionStepGuid;
}