using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentNHibernate.Utils;
using TrueOrFalse.Frontend.Web.Code;

namespace TrueOrFalse
{
    [Serializable]
    public class QuestionHistory : HistoryBase<QuestionHistoryItem>
    {
        public QuestionHistory()
        {
            _size = 5;
        }
    }

    [Serializable]
    public class QuestionHistoryItem : HistoryItemBase
    {
        public int Id { get; private set; }
        public int SetId { get; private set; }
        public string Text { get; private set; }
        public string Solution { get; private set; }
        public HistoryItemType Type { get; set; }
        
        public QuestionSearchSpec SearchSpec { get; set; }

        public Func<UrlHelper, string> Link;

        public Question Question;
        public Set Set;

        public QuestionHistoryItem(
            Set set,
            Question question,
            HistoryItemType type = HistoryItemType.Any)
        {
            Set = set;
            SetId = set.Id;
            Type = type;
            
            FillQuestionFields(question);
        }

        public QuestionHistoryItem(
            Question question, 
            QuestionSearchSpec seachSpec,
            HistoryItemType type = HistoryItemType.Any)
        {
            Type = type;
            FillQuestionFields(question);
            
            SearchSpec = QuestionSearchSpecSession.CloneAndAddToSession(seachSpec, this);
        }

        public QuestionHistoryItem(Question question, HistoryItemType type)
        {
            Type = type;
            
            FillQuestionFields(question);
        }

        private void FillQuestionFields(Question question)
        {
            Question = question;
            Id = question.Id;
            Text = question.Text;
            Solution = question.Solution;            
        }
    }
}