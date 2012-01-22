using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class AnswerHistoryLog : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;

        public AnswerHistoryLog(AnswerHistoryRepository answerHistoryRepository)
        {
            _answerHistoryRepository = answerHistoryRepository;
        }

        public void Run(Question question, AnswerQuestionResult result, int userId)
        {
            var answerHistory = new AnswerHistory();
            answerHistory.AnswerText = result.AnswerGiven;
            answerHistory.QuestionId = question.Id;
            answerHistory.UserId = userId;
            _answerHistoryRepository.Create(answerHistory);
        }
    }
}
