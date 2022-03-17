using System;
using System.Collections.Generic;

[Serializable]
public class LearningSessionConfig
{
    public int CategoryId { get; set; }
    public CategoryCacheItem Category { get; set; }
    public int MaxQuestionCount { get; set; } = 0;
    /// <summary>
    /// Currently logged in user
    /// </summary>
    public int CurrentUserId { get; set; }
    public bool IsInTestMode { get; set; }
    public bool IsInLearningTab { get; set; }
    public QuestionOrder QuestionOrder { get; set; } = QuestionOrder.SortByEasiest;
    public bool AnswerHelp { get; set; }
    public RepetitionType Repetition { get; set; } = RepetitionType.None;
    /// <summary>
    /// User is not logged in
    /// </summary>
    public bool IsAnonymous() => CurrentUserId == -1;

    public bool IsMyWorld() => UserCache.GetItem(CurrentUserId).IsFiltered;

    public bool InWuwi { get; set; } = true;
    public bool NotInWuwi { get; set; } = true;
    public bool CreatedByCurrentUser { get; set; } = true;
    public bool NotCreatedByCurrentUser { get; set; } = true;
    public bool PrivateQuestions { get; set; } = true;
    public bool PublicQuestions { get; set; } = true;

    public bool NotLearned { get; set; } = true;
    public bool NeedsLearning { get; set; } = true;
    public bool NeedsConsolidation { get; set; } = true;
    public bool Solid { get; set; } = true;
}

public enum QuestionOrder
{
    SortByEasiest,
    SortByHardest,
    SortByPersonalHardest,
    Random
}

public enum RepetitionType
{
    None,
    Normal,
    Leitner
}

