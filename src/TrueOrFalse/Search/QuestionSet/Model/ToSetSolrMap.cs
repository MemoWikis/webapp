using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    public class ToSetSolrMap
    {
        public static SetSolrMap Run(QuestionSet set)
        {
            var result = new SetSolrMap();
            result.Text = set.Text;
            result.Name = set.Name;
            result.CreatorId = set.Creator.Id;
            result.AllQuestionsTitles = set.QuestionsInSet.Select(q => q.Question.Text).Aggregate((a, b) => a + " " + b);
            result.AllQuestionsBodies = set.QuestionsInSet.Select(q => q.Question.Description).Aggregate((a, b) => a + " " + b);

            return result;
        }
    }
}
