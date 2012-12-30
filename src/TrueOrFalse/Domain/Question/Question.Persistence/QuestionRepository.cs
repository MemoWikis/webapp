using System;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionRepository : RepositoryDb<Question> 
    {
        public QuestionRepository(ISession session) : base(session)
        {
        }

        public override void Update(Question question)
        {
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
        }
    }
}