using System;
using System.Collections.Generic;
using TrueOrFalse.Core;

namespace TrueOrFalse.Core
{
    public class QuestionRepositoryDemoData : IQuestionRepository
    {
        public void Create(Question question)
        {
            question.Id = QuestionDemoData.All().Count + 1;
            QuestionDemoData.All().Add(question);
        }

        public Question GetById(int id)
        {
            return QuestionDemoData.All().GetById(id);
        }

        public IList<Question> GetAll()
        {
            return QuestionDemoData.All();
        }

        
    }
}