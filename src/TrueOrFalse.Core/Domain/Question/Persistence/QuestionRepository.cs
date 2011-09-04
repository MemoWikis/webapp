using System;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class QuestionRepository : RepositoryDb<Question> 
    {
        public QuestionRepository(ISession session) : base(session)
        {
        }

        public override void Update(Question question)
        {
            foreach (Answer answer in question.Answers)
                answer.DateModified = DateTime.Now;
            
            base.Update(question);
            Flush();            
        }

        public override void Create(Question question)
        {
            foreach(Answer answer in question.Answers)
                if(answer.Id == 0)
                {
                    answer.DateCreated = DateTime.Now;
                    answer.DateModified = DateTime.Now;
                }
            
            base.Create(question);
            Flush();
        }
    }
}