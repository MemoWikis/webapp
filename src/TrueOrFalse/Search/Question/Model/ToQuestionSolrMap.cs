using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Search
{
    public class ToQuestionSolrMap
    {
        public static QuestionSolrMap Run(Question question, IEnumerable<QuestionValuation> valuations)
        {
            var result = new QuestionSolrMap
                {
                    Id = question.Id,
                    CreatorId = question.Creator.Id,
                    ValuatorIds = valuations.Where(v => v.RelevancePersonal != -1).Select(x => x.UserId).ToList(),
                    Text = question.Text,
                    Description = question.Description,
                    Solution = question.Solution,
                    SolutionType = (int) question.SolutionType,
                    Categories = question.Categories.Select(c => c.Name).ToArray(),
                    AvgQuality = question.TotalQualityAvg,
                    Views = question.TotalViews,
                    DateCreated = question.DateCreated
                };

            return result;
        }
    }
}
