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
        private readonly CBForAllUsersCalculator _cbForAllUsersCalc;

        public RecalcCBForQuestions(
            QuestionRepository questionRepo,
            AnswerHistoryRepository answerHistoryRepo,
            CBForAllUsersCalculator cbForAllUsersCalc
            )
        {
            _questionRepo = questionRepo;
            _answerHistoryRepo = answerHistoryRepo;
            _cbForAllUsersCalc = cbForAllUsersCalc;
        }

        public void Run()
        {
            
            foreach (var question in _questionRepo.GetAll())
            {
                question.CorrectnessProbability = 
                    _cbForAllUsersCalc.Run(_answerHistoryRepo.GetBy(question.Id));

                _questionRepo.Update(question);
            }
        }
    }
}
