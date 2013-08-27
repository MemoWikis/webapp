using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _solrOperations;

        public SearchIndexQuestion(ISolrOperations<QuestionSolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(Question question)
        {
            _solrOperations.Add(ToQuestionSolrMap.Run(question));
            _solrOperations.Commit();
        }

        public void Delete(Question question)
        {
            _solrOperations.Delete(ToQuestionSolrMap.Run(question));
            _solrOperations.Commit();
        }
    }
}