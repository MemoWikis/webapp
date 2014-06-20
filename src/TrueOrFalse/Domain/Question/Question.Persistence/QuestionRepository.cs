using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Remotion.Linq.Clauses.ResultOperators;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class QuestionRepository : RepositoryDb<Question> 
    {
        private readonly SearchIndexQuestion _searchIndexQuestion;

        public QuestionRepository(ISession session, SearchIndexQuestion searchIndexQuestion) : base(session)
        {
            _searchIndexQuestion = searchIndexQuestion;
        }

        public override void Update(Question question)
        {
            _searchIndexQuestion.Update(question);
            base.Update(question);
            Flush();
            Sl.Resolve<UpdateQuestionCountForCategory>().Run(question.Categories);
        }

        public override void Create(Question question)
        {
            if(question.Creator == null)
                throw new Exception("no creator");

            base.Create(question);
            Flush();
            Sl.Resolve<UpdateQuestionCountForCategory>().Run(question.Categories);
            _searchIndexQuestion.Update(question);
        }

        public override void Delete(Question question)
        {
            _searchIndexQuestion.Delete(question);
            base.Delete(question);
        }

        public IList<Question> GetForCategory(int categoryId, int resultCount)
        {
            return _session.QueryOver<Question>()
                .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
                .ThenBy(x => x.DateCreated).Desc
                .JoinQueryOver<Category>(q => q.Categories)
                .Where(c => c.Id == categoryId)
                .Take(resultCount)
                .List<Question>();
        }

        public IList<Question> GetByIds(List<int> questionIds)
        {
            return GetByIds(questionIds.ToArray());
        }

        public override IList<Question> GetByIds(params int[] questionIds)
        {
            var tmpResult = base.GetByIds(questionIds);

            var result = new List<Question>();
            for (int i = 0; i < questionIds.Length; i++)
            {
                if (tmpResult.Any(q => q.Id == questionIds[i]))
                    result.Add(tmpResult.First(q => q.Id == questionIds[i]));
            }

            return result;
        }

    }
}