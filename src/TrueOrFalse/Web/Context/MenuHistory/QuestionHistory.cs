using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;

namespace TrueOrFalse
{
    public class QuestionHistory : HistoryBase<QuestionHistoryItem>
    {
    }

    public class QuestionHistoryItem : HistoryItemBase
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public HistoryItemType Type { get; set; }
        public QuestionSearchSpec SearchSpec { get; set; }

        public QuestionHistoryItem(
            Question question, 
            QuestionSearchSpec seachSpec,
            HistoryItemType type = HistoryItemType.Any)
        {
            SearchSpec = QuestionSearchSpecSession.CloneAndAddToSession(seachSpec);
            Id = question.Id;
            Text = question.Text;
            Type = type;
        }
    }
}
