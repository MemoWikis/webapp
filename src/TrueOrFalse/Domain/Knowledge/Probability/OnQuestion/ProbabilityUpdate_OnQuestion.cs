using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_OnQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepo;
        private readonly AnswerHistoryRepository _answerHistoryRepo;
        private readonly ProbabilityCalc_OnQuestion _probabilityCalcOnQuestion;

        public ProbabilityUpdate_OnQuestion(
            QuestionRepository questionRepo,
            AnswerHistoryRepository answerHistoryRepo,
            ProbabilityCalc_OnQuestion probabilityCalcOnQuestion
            )
        {
            _questionRepo = questionRepo;
            _answerHistoryRepo = answerHistoryRepo;
            _probabilityCalcOnQuestion = probabilityCalcOnQuestion;
        }

        public void Run()
        {
            
            foreach (var question in _questionRepo.GetAll())
            {
                question.CorrectnessProbability = 
                    _probabilityCalcOnQuestion.Run(_answerHistoryRepo.GetBy(question.Id));

                _questionRepo.Update(question);
            }
        }
    }
}
