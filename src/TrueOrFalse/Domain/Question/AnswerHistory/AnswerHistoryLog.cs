using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public class AnswerHistoryLog : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;

        public AnswerHistoryLog(AnswerHistoryRepository answerHistoryRepository)
        {
            _answerHistoryRepository = answerHistoryRepository;
        }

        public void Run(Question question, AnswerQuestionResult answerQuestionResult, int userId)
        {
            var answerHistory = new AnswerHistory();
            answerHistory.QuestionId = question.Id;
            answerHistory.UserId = userId;
            answerHistory.AnswerText = answerQuestionResult.AnswerGiven;
            answerHistory.AnswerredCorrectly = answerQuestionResult.IsCorrect;
            _answerHistoryRepository.Create(answerHistory);
        }
    }
}
