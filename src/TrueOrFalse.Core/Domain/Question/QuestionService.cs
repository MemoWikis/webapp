using System;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests.Answer
{
    public class QuestionService : IQuestionService
    {
        public void Create(Question question)
        {
            var sessionFactory = SessionFactory.CreateSessionFactory();
            
            using(var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(question);
            }
        }
    }
}