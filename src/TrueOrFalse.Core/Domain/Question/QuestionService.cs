using System;
using NHibernate;
using TrueOrFalse.Core;

namespace TrueOrFalse.Core
{
    public abstract class QuestionService : IQuestionService
    {
        private readonly ISession _session;

        public QuestionService(ISession session)
        {
            _session = session;
        }

        public void Create(Question question)
        {
            _session.SaveOrUpdate(question);
        }

    	public QuestionList GetAll()
    	{
    	    return new QuestionList(
    	        _session.CreateCriteria(typeof (Question)).List<Question>());
    	}
    }
}