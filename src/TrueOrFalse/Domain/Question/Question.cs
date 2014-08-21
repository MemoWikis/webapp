using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse
{
    [DebuggerDisplay("Id={Id} Name={Text}")]
    [Serializable]
    public class Question : DomainEntity
    {
        public virtual string Text { get; set; }
        public virtual string TextExtended { get; set; }
        public virtual string Description { get; set; }
        public virtual string Solution { get; set; }
        public virtual SolutionType SolutionType { get; set; }
        public virtual string SolutionMetadataJson { get; set; }

        public virtual IList<Category> Categories { get; set; }
        public virtual IList<Reference> References { get; set; }
        public virtual QuestionVisibility Visibility { get; set; }

        public virtual User Creator { get; set; }

        public virtual int TotalTrueAnswers { get; set; }
        public virtual int TotalFalseAnswers { get; set; }

        public virtual int CorrectnessProbability { get; set; }

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

        public virtual int SetsAmount { get; set; }
        public virtual string SetsTop5Json { get; set; }

        public virtual IList<SetMini> SetTop5Minis
        {
            get
            {
                if(String.IsNullOrEmpty(SetsTop5Json))
                    return new List<SetMini>();
                return JsonConvert.DeserializeObject<List<SetMini>>(SetsTop5Json);
            }
            set { SetsTop5Json = JsonConvert.SerializeObject(value);  }
        }

        public Question()
        {
            Categories = new List<Category>();
            References = new List<Reference>();
        }

        public virtual string GetShortTitle(int length = 96) 
        {
            return Text.TruncateAtWord(length);
        }

        public virtual bool IsPrivate()
        {
            return Visibility != QuestionVisibility.All;
        }
    }
}
