using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search
{
    public class MeiliSearchToQuestionMap
    {
        public static MeiliSearchQuestionMap Run(Question question, IList<QuestionValuationCacheItem> questionValuations)
        {
            return new MeiliSearchQuestionMap
            {
                Id = question.Id,
                Categories = question.Categories.Select(c => c.Name).ToList(),
                CategoryIds = question.Categories.Select(c => c.Id).ToList(),
                AvgQuality = question.TotalQualityAvg,
                CreatorId = question.Creator == null ? -1 : question.Creator.Id,
                Description = question.Description,
                DateCreated = question.DateCreated,
                IsPrivate = question.IsPrivate(),
                Solution = question.Solution,
                SolutionType = (int)question.SolutionType,
                Text = question.Text,
                Valuation = questionValuations.Count(),
                ValuatorIds = questionValuations.Select(qv => qv.Id).ToList()
            };
        }
    }
}
