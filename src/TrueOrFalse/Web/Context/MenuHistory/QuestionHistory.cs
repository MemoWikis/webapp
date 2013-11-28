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
    public class QuestionHistory : HistoryBase<QuestionHistoryItem>
    {
    }

    public class QuestionHistoryItem : HistoryItemBase
    {
        public int Id { get; private set; }
        public int SetId { get; private set; }
        public string Text { get; private set; }
        public HistoryItemType Type { get; set; }
        
        public QuestionSearchSpec SearchSpec { get; set; }

        public Func<UrlHelper, string> Link;

        public QuestionHistoryItem(
            Set set,
            Question question,
            HistoryItemType type = HistoryItemType.Any)
        {
            Id = question.Id;
            SetId = set.Id;
            Text = question.Text;
            Type = type;

            Link = url => Links.AnswerQuestion(url, question, set);
        }

        public QuestionHistoryItem(
            Question question, 
            QuestionSearchSpec seachSpec,
            HistoryItemType type = HistoryItemType.Any)
        {
            SearchSpec = QuestionSearchSpecSession.CloneAndAddToSession(seachSpec);
            Id = question.Id;
            Text = question.Text;
            Type = type;

            Link = url => Links.AnswerQuestion(url, seachSpec);
        }
    }
}