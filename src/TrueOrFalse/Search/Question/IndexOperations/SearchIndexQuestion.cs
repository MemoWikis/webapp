using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _solrOperations;

        private QuestionValuationRepository __questionValuationRepo;

        private QuestionValuationRepository _questionValuationRepo
        {
            get
            {
                if (__questionValuationRepo == null)
                    __questionValuationRepo = Sl.Resolve<QuestionValuationRepository>();

                return __questionValuationRepo;
            }
        }

        public SearchIndexQuestion(ISolrOperations<QuestionSolrMap> solrOperations)
        {
            _solrOperations = solrOperations;
        }

        public void Update(IList<Question> questions)
        {
            foreach (var question in questions)
                _solrOperations.Add(ToQuestionSolrMap.Run(
                    question, _questionValuationRepo.GetBy(question.Id)));
        }

        public void Update(Question question, bool commitDelayed = true)
        {
            if (question == null)
                return;

            if (!commitDelayed)
                _solrOperations.Add(ToQuestionSolrMap.Run(question, _questionValuationRepo.GetBy(question.Id)));
            else
            {
                var sp = Stopwatch.StartNew();
                Loggly.Send("Question2SearchIndex-Start: " + sp.Elapsed, LogglyCategories.Performance);

                var solrQuestion = ToQuestionSolrMap.Run(question, _questionValuationRepo.GetBy(question.Id));
                ExecAsync.Go(() => _solrOperations.Add(solrQuestion, new AddParameters { CommitWithin = 10000 }));

                Loggly.Send("Question2SearchIndex-Stop: " + sp.Elapsed, LogglyCategories.Performance);
                
            }

            _solrOperations.Commit();
        }

        public void Delete(Question question)
        {
            _solrOperations.Delete(ToQuestionSolrMap.Run(question, _questionValuationRepo.GetBy(question.Id)));
            _solrOperations.Commit();
        }


    }
}