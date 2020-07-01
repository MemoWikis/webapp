using System;

[Serializable]
public class LearningSessionConfig
{
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int MaxQuestions { get; set; }
    public int UserId { get; set; }
    public bool IsInTestMode { get; set; }
    public bool IsInLearningTab { get; set; } 
    public bool IsWishSession { get; set; }
    public int MinProbability { get; set; }
    public int MaxProbability { get; set; }
    public int QuestionOrder { get; set; }
    public string Mode { get; set; }

    /// <summary>
    /// User is not logged in
    /// </summary>
    public bool IsAnonymous() => UserId == -1;
}

