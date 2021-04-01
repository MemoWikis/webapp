using System;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("QuestionId={Question.Id}, IsInWuwi: {IsInWishKnowledge()}")]
public class QuestionValuation : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }

    public virtual User User { get; set; }
    public virtual Question Question { get; set; }

    public virtual int Quality { get; set; }
    public virtual int RelevancePersonal { get; set; }
    public virtual int RelevanceForAll { get; set; }

    public virtual DateTime DateCreated { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual KnowledgeStatus KnowledgeStatus { get; set; }

    public virtual bool IsSetQuality(){ return Quality != -1;}
    public virtual bool IsSetRelevanceForAll(){ return RelevanceForAll != -1;}
    public virtual bool IsInWishKnowledge() { return RelevancePersonal > -1; }

    public QuestionValuation()
    {
        Quality = -1;
        RelevancePersonal = -1;
        RelevanceForAll = -1;
    }

    public virtual QuestionValuationCacheItem ToCacheItem()
    {
        return new QuestionValuationCacheItem
        {
            Id = Id,
            User = User,
            CorrectnessProbability = CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = CorrectnessProbabilityAnswerCount,
            KnowledgeStatus = KnowledgeStatus,
            IsInWishKnowledge = IsInWishKnowledge(),
            DateCreated = DateCreated,
            Question = Question,
        };
    }
}