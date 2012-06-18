using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core
{
    public class Question : DomainEntity
    {
        public virtual string Text { get; set; }
        public virtual string Description { get; set; }
        public virtual string Solution { get; set; }
        public virtual IList<Category> Categories { get; set; }
        public virtual QuestionVisibility Visibility { get; set; }
        public virtual QuestionSolutionType SolutionType { get; set; }
        public virtual User Creator { get; set; }

        private int _totalTrueAnswers;
        public virtual int TotalTrueAnswers
        {
            get { return _totalTrueAnswers; }
            set { _totalTrueAnswers = value; }
        }

        private int _totalFalseAnswers;
        public virtual int TotalFalseAnswers
        {
            get { return _totalFalseAnswers; }
            set { _totalFalseAnswers = value; }
        }

        public Question()
        {
            Categories = new List<Category>();
        }

        public virtual string GetShortTitle()
        {
            return Text.TruncateAtWord(96);
        }

        public virtual int TotalAnswers() { return TotalFalseAnswers + TotalTrueAnswers; }
        public virtual int TotalTrueAnswersPercentage()
        {
            if (TotalAnswers() == 0) return 0;
            if (TotalTrueAnswers == 0) return 0;
            return Convert.ToInt32(((decimal)TotalTrueAnswers / (decimal)TotalAnswers()) * 100);
        }
        public virtual int TotalFalseAnswerPercentage()
        {
            if (TotalAnswers() == 0) return 0;
            if (TotalFalseAnswers == 0) return 0;
            return Convert.ToInt32(((decimal)TotalFalseAnswers / (decimal)TotalAnswers()) * 100);
        }

    }
}
