using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class AddToQuestionSet : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionSetRepository _questionSetRepo;
        private readonly QuestionRepository _questionRepo;

        public AddToQuestionSet(QuestionSetRepository questionSetRepo, QuestionRepository questionRepo){
            _questionSetRepo = questionSetRepo;
            _questionRepo = questionRepo;
        }

        public AddToQuestionSetResult Run(int[] questionIds, int questionSet)
        {
            return Run(
                _questionRepo.GetByIds(questionIds), 
                _questionSetRepo.GetById(questionSet));
        }

        public AddToQuestionSetResult Run(IList<Question> questions, QuestionSet questionSet)
        {
            var nonAddedQuestions = new List<Question>();
            foreach (var question in questions)
            {
                if(questionSet.QuestionsInSet.Any(q => q.Question.Id == question.Id))
                    nonAddedQuestions.Add(question);
                else
                    questionSet.Add(question);
            }

            _questionSetRepo.Update(questionSet);

            return new AddToQuestionSetResult
                {
                    AmountAddedQuestions = questions.Count() - nonAddedQuestions.Count(),
                    AmountOfQuestionsAlreadyInSet = nonAddedQuestions.Count(),
                    Set = questionSet
                };
        }
    }
}
