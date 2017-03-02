﻿using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search
{
    public class ToCategorytSolrMap
    {
        public static CategorySolrMap Run(Category category, IEnumerable<CategoryValuation> valuations)
        {
            var result = new CategorySolrMap();
            result.Id = category.Id;
            result.Name = category.Name;
            result.Description = category.Description;

            result.CreatorId = category.Creator.Id;

            result.ValuatorIds = valuations.Where(v => v.RelevancePersonal != -1).Select(x => x.UserId).ToList();
            result.ValuationsCount = category.TotalRelevancePersonalEntries;

            result.QuestionCount = category.CountQuestions;
            result.DateCreated = category.DateCreated;

            return result;
        }
    }
}
