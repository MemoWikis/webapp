using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse
{
    public class Question : DomainEntity
    {
        public virtual string Text { get; set; }
        public virtual string TextExtended { get; set; }
        public virtual string Description { get; set; }
        public virtual string Solution { get; set; }
        public virtual SolutionType SolutionType { get; set; }
        public virtual string SolutionMetadataJson { get; set; }

        public virtual IList<Category> Categories { get; set; }
        public virtual QuestionVisibility Visibility { get; set; }

        public virtual User Creator { get; set; }

        public virtual int TotalTrueAnswers { get; set; }
        public virtual int TotalFalseAnswers { get; set; }

        public virtual int TotalAnswers() { return TotalFalseAnswers + TotalTrueAnswers; }
        public virtual int TotalTrueAnswersPercentage()
        {
            if (TotalAnswers() == 0) return 0;
            if (TotalTrueAnswers == 0) return 0;
            return Convert.ToInt32(((decimal)TotalTrueAnswers / TotalAnswers()) * 100);
        }
        public virtual int TotalFalseAnswerPercentage()
        {
            if (TotalAnswers() == 0) return 0;
            if (TotalFalseAnswers == 0) return 0;
            return Convert.ToInt32(((decimal)TotalFalseAnswers / TotalAnswers()) * 100);
        }

        public virtual int TotalQualityAvg { get; set; }
        public virtual int TotalQualityEntries { get; set; }

        public virtual int TotalRelevanceForAllAvg { get; set; }
        public virtual int TotalRelevanceForAllEntries { get; set; }

        public virtual int TotalRelevancePersonalAvg { get; set; }
        public virtual int TotalRelevancePersonalEntries { get; set; }

        public virtual int TotalViews { get; set; }

        public Question()
        {
            Categories = new List<Category>();
        }

        public virtual string GetShortTitle()
        {
            return Text.TruncateAtWord(96);
        }
    }
}
