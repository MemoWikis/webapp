using System;

[Serializable]
public class LearningSessionConfig
{
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int MaxQuestions = 2; 
    public bool OnlyWuwi { get; set; }
    public int UserId = -1;
    public bool IsInTestMode { get; set; }
    public bool IsInLearningTab { get; set; } = false;
    public bool IsWishSession { get; set; }

    /// <summary>
    /// User is not logged in
    /// </summary>
    public bool IsAnonymous() => UserId == -1;
    public bool ReAddStepsToEnd() => !IsAnonymous() && !IsInTestMode;
}
