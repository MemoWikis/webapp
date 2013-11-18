using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class QuestionValuationRepository : RepositoryDb<QuestionValuation> 
    {
        private readonly SearchIndexQuestion _searchIndexQuestion;
        private readonly QuestionRepository _questionRepository;

        public QuestionValuationRepository(
            ISession session, 
            SearchIndexQuestion searchIndexQuestion,
            QuestionRepository questionRepository) : base(session)
        {
            _searchIndexQuestion = searchIndexQuestion;
            _questionRepository = questionRepository;
        }

        public QuestionValuation GetBy(int questionId, int userId)
        {
            return _session.QueryOver<QuestionValuation>()
                           .Where(q => q.UserId == userId && q.QuestionId == questionId)
                           .SingleOrDefault();
        }

        public IList<QuestionValuation> GetBy(int questionId)
        {
            return _session.QueryOver<QuestionValuation>()
                           .Where(q => q.QuestionId == questionId)
                           .List<QuestionValuation>();
        }

        public IList<QuestionValuation> GetBy(IList<int> questionIds, int userId)
        {
            if(!questionIds.Any())
                return new List<QuestionValuation>();

            var sb = new StringBuilder();
            sb.Append("SELECT * FROM QuestionValuation WHERE UserId = " + userId + " ");
            sb.Append("AND (QuestionId = " + questionIds[0]);

            for(int i = 1; i < questionIds.Count(); i++){
                sb.Append(" OR QuestionId = " + questionIds[i]);
            }
            sb.Append(")");

            Console.Write(sb.ToString());

            return _session.CreateSQLQuery(sb.ToString())
                           .SetResultTransformer(Transformers.AliasToBean(typeof(QuestionValuation)))
                           .List<QuestionValuation>();
        }

        //_searchIndexQuestion

        public override void Create(IList<QuestionValuation> questionValuations)
        {
            base.Create(questionValuations);
            _searchIndexQuestion.Update(_questionRepository.GetByIds(questionValuations.QuestionIds().ToArray()));
        }

        public override void Create(QuestionValuation questionValuation)
        {
            base.Create(questionValuation);
            _searchIndexQuestion.Update(_questionRepository.GetById(questionValuation.QuestionId));
        }

        public override void CreateOrUpdate(QuestionValuation questionValuation)
        {
            base.CreateOrUpdate(questionValuation);
            _searchIndexQuestion.Update(_questionRepository.GetById(questionValuation.Id));
        }

        public override void Update(QuestionValuation questionValuation)
        {
            base.Update(questionValuation);
            _searchIndexQuestion.Update(_questionRepository.GetById(questionValuation.Id));
        }


    }
}
