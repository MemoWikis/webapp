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
    public bool InWishknowledge { get; set; }
    public int MinProbability { get; set; }
    public int MaxProbability { get; set; } = 100; 
    public int QuestionOrder { get; set; }
    public bool AllQuestions { get; set; }
    public bool IsNotQuestionInWishKnowledge { get; set; }
    public bool SafeLearningSessionOptions { get; set; }
    public bool AnswerHelp { get; set; }
    public bool Repetitions { get; set; }
    public bool RandomQuestions { get; set; }
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

