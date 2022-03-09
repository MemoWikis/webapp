﻿using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search
{
    public class ToCategorytSolrMap
    {
        public static CategorySolrMap Run(Category category, IEnumerable<CategoryValuation> valuations)
        {
            var categoryCacheItem = EntityCache.GetCategory(category) ?? CategoryCacheItem.ToCacheCategory(category);

            var result = new CategorySolrMap();
            result.Id = category.Id;
            result.Name = category.Name;
            result.Description = category.Description;

            if (category.Creator != null)
                result.CreatorId = category.Creator.Id;
            else
                result.CreatorId = -1;

            result.ValuatorIds = valuations.Where(v => v.RelevancePersonal != -1).Select(x => x.UserId).ToList();
            result.ValuationsCount = categoryCacheItem.TotalRelevancePersonalEntries;
            result.QuestionCount = categoryCacheItem.CountQuestionsAggregated;
            result.DateCreated = category.DateCreated;

            return result;
        }
    }
}
