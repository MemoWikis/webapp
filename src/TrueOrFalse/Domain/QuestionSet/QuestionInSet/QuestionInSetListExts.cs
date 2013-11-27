using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;

namespace TrueOrFalse
{
    public static class QuestionInSetListExts
    {
        public static int GetIndex(this IEnumerable<QuestionInSet> questionsInSets, int questionId)
        {
            var result = -1;
            questionsInSets.Select((x, index) =>
            {
                if (x.Question.Id == questionId)
                    result = index;

                return -1;
            }).ToList();

            return result;
        }

        public static QuestionInSet GetNextTo(this IEnumerable<QuestionInSet> questionsInSets, int questionId)
        {
            var index = questionsInSets.GetIndex(questionId);
            return questionsInSets.ToList()[index + 1];
        }

        public static QuestionInSet GetPreviousTo(this IEnumerable<QuestionInSet> questionsInSets, int questionId)
        {
            var index = questionsInSets.GetIndex(questionId);
            return questionsInSets.ToList()[index - 1];
        }
    }
}
