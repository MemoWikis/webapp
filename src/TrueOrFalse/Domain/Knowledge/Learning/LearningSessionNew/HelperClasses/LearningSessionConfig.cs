public class LearningSessionConfig
{
    public int CategoryId;
    public int MaxQuestions;
    public bool OnlyWuwi;
    public int UserId;

    /// <summary>
    /// User is not logge in
    /// </summary>
    public bool IsAnonymous()=> UserId == -1;

    public bool ReAddStepsToEnd() => !IsAnonymous();
}
