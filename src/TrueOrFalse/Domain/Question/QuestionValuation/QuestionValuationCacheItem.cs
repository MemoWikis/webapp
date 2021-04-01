using System;
using System.Diagnostics;

[DebuggerDisplay("QuestionId={Question.Id}, IsInWuwi: {IsInWishKnowledge}")]
public class QuestionValuationCacheItem
{
    public int Id;

    public User User;
    public Question Question;
    public DateTime DateCreated;

    public int CorrectnessProbability;
    public int CorrectnessProbabilityAnswerCount;

    public KnowledgeStatus KnowledgeStatus;

    public bool IsInWishKnowledge;
}
