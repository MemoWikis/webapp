using System;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class QuestionRepository : RepositoryDb<Question> 
    {
        private readonly SearchIndexQuestion _searchIndexQuestion;

        public QuestionRepository(ISession session, SearchIndexQuestion searchIndexQuestion) : base(session){
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
            base.Delete(question.Id);
        }
    }
}