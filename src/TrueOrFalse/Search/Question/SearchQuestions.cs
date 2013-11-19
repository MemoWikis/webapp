﻿using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _searchOperations;

        public SearchQuestions(ISolrOperations<QuestionSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchQuestionsResult Run(string searchTerm){
            return Run(searchTerm, new Pager());
        }

        public SearchQuestionsResult Run(
            string searchTerm, 
            Pager pager,
            int creatorId = -1, 
            int valuatorId = -1)
        {
            var sqb = new SearchQueryBuilder()
                .Add("FullTextStemmed", searchTerm)
                .Add("FullTextExact", searchTerm)
                .Add("Categories", searchTerm)
                .Add("CreatorId", creatorId != -1 ? creatorId.ToString() : null, isMustHave: true, exact: true)
                .Add("ValuatorIds", valuatorId != -1 ? valuatorId.ToString() : null, isMustHave: true, exact: true);

            var queryResult = _searchOperations.Query(sqb.ToString(),
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true},
                                                            ExtraParams = new Dictionary<string, string> { { "qt", "dismax" } }
                                                      });

            var result = new SearchQuestionsResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;

            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.QuestionIds.Add(resultItem.Id);

            return result;
        }
    }
}