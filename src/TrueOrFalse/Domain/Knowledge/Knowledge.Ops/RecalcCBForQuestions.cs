using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class RecalcCBForQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepo;
        private readonly AnswerHistoryRepository _answerHistoryRepo;
        private readonly ProbabilityForAllUsersCalc _probabilityForAllUsersCalc;

        public RecalcCBForQuestions(
            QuestionRepository questionRepo,
            AnswerHistoryRepository answerHistoryRepo,
            ProbabilityForAllUsersCalc probabilityForAllUsersCalc
            )
        {
            _questionRepo = questionRepo;
            _answerHistoryRepo = answerHistoryRepo;
            _probabilityForAllUsersCalc = probabilityForAllUsersCalc;
        }

        public void Run()
        {
            
            foreach (var question in _questionRepo.GetAll())
            {
                question.CorrectnessProbability = 
                    _probabilityForAllUsersCalc.Run(_answerHistoryRepo.GetBy(question.Id));

                _questionRepo.Update(question);
            }
        }
    }
}
