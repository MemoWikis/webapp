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

        public SearchQuestionsResult Run(string searchTearm){
            return Run(searchTearm, new Pager());
        }

        public SearchQuestionsResult Run(string searchTearm, Pager pager)
        {
            var queryResult = _searchOperations.Query("FullTextStemmed:" + searchTearm + " " +
                                                      "FullTextExact:" + searchTearm + " " +
                                                      "Categories:" + searchTearm, 
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true},
                                                            
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