using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse.Web;

namespace TrueOrFalse
{
    public class QuestionSetHistory : HistoryBase<QuestionSetHistoryItem>
    {
    }

    public class QuestionSetHistoryItem : HistoryItemBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public HistoryItemType Type { get; set; }

        public QuestionSetHistoryItem(QuestionSet questionSet, HistoryItemType type = HistoryItemType.Any)
        {
            Id = questionSet.Id;
            Name = questionSet.Name;
            Type = type;
        }
    }
}
