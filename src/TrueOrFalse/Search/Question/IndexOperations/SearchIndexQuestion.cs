using System;
using System.Collections.Generic;
using System.Threading;
using Quartz;
using Serilog;
using SolrNet;
using SolrNet.Commands;

namespace TrueOrFalse.Search
{
    public class SearchIndexQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _solrOperations;

        private QuestionValuationRepo __questionValuationRepo;

        private QuestionValuationRepo _questionValuationRepo
        {
            get
            {
                if (__questionValuationRepo == null)
                    __questionValuationRepo = Sl.Resolve<QuestionValuationRepo>();

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
            {
                if(question.IsWorkInProgress)
                    continue;

                _solrOperations.Add(ToQuestionSolrMap.Run(
                    question, _questionValuationRepo.GetActiveInWishknowledgeFromCache(question.Id)));                
            }
        }

        public void Update(Question question, bool softCommit = true)
        {
            if (question == null)
                return;

            if (question.IsWorkInProgress)
                return;

            if (!softCommit || JobExecute.CodeIsRunningInsideAJob)
            {
                _solrOperations.Add(ToQuestionSolrMap.Run(question, _questionValuationRepo.GetActiveInWishknowledgeFromCache(question.Id)));
                _solrOperations.Commit();    
            }
            else
            {
                Logg.r().Information("SearchIndexQuestion {ThreadId} before Async", Thread.CurrentThread.ManagedThreadId);
                AsyncExe.Run(() =>
                {
                    Logg.r().Information("SearchIndexQuestion {ThreadId} inside Async", Thread.CurrentThread.ManagedThreadId);

                    JobExecute.Run((scope) =>
                    {
                        Logg.r().Information("SearchIndexQuestion {ThreadId} inside Job", Thread.CurrentThread.ManagedThreadId);

                        var solrQuestion = ToQuestionSolrMap.Run(question,
                            Sl.QuestionValuationRepo.GetActiveInWishknowledgeFromCache(question.Id));
                        _solrOperations.Add(solrQuestion, new AddParameters { CommitWithin = 5000 });

                    }, "UpdateQuestionSolrJob", writeLog: false);
                });
            }
        }

        public void UpdateQuestionView(int questionId, int totalViews, int? creatorId)
        {
            _solrOperations.AtomicUpdate(
                questionId.ToString(), 
                new[]
                {
                    new AtomicUpdateSpec("Views", AtomicUpdateType.Set, totalViews),
                    new AtomicUpdateSpec("CreatorId", AtomicUpdateType.Set, creatorId ?? -1)
                }
            );
            _solrOperations.Commit();
        }

        public void Delete(Question question)
        {
            _solrOperations.Delete(ToQuestionSolrMap.Run(question, _questionValuationRepo.GetActiveInWishknowledgeFromCache(question.Id)));
            _solrOperations.Commit();
        }
    }
}