using System;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionValuation : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }

        public virtual int UserId { get; set; }
        public virtual int QuestionId { get; set; }

        public virtual int Quality { get; set; }
        public virtual int RelevancePersonal { get; set; }
        public virtual int RelevanceForAll { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual int CorrectnessProbability { get; set; }

        public virtual KnowledgeStatus KnowledgeStatus { get; set; }

        public virtual bool IsSetQuality(){ return Quality != -1;}
        public virtual bool IsSetRelevanceForAll(){ return RelevanceForAll != -1;}
        public virtual bool IsSetRelevancePersonal() { return RelevancePersonal != -1; }

        public QuestionValuation()
        {
            Quality = -1;
            RelevancePersonal = - 1;
            RelevanceForAll = -1;
        }
    }
}
