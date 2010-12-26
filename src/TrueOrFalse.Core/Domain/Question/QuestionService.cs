using System;
using TrueOrFalse.Core;

namespace TrueOrFalse.Core
{
    public class QuestionService : IQuestionService
    {
        public QuestionService()
        {
            
        }

        public void Create(Question question)
        {
            var sessionFactory = SessionFactory.CreateSessionFactory();
            
            using(var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(question);
            }
        }

    	public QuestionList GetAll()
    	{
			var sessionFactory = SessionFactory.CreateSessionFactory();

			using (var session = sessionFactory.OpenSession())
			{
				return new QuestionList(
					session.CreateCriteria(typeof (Question)).List<Question>()
				);
			}
    	}


    }
}