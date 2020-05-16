using System;

[Serializable]
public class LearningSessionConfig
{
    public int CategoryId { get; set; }
    public int MaxQuestions = 10; 
    public bool OnlyWuwi { get; set; }
    public int UserId { get; set; }
    public bool IsInTestmode { get; set; }
    public bool IsInLearningTab { get; set; } = false;

    /// <summary>
    /// User is not logged in
    /// </summary>
    public bool IsAnonymous() => UserId == -1;
    public bool ReAddStepsToEnd() => !IsAnonymous();
}
