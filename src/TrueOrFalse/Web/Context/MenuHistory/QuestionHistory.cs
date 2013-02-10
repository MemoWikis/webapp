using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public QuestionHistoryItem(Question question, HistoryItemType type = HistoryItemType.Any)
        {
            Id = question.Id;
            Text = question.Text;
            Type = type;
        }
    }
}
