using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class AnswerQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerHistoryLog _answerHistoryLog;

        public AnswerQuestion(QuestionRepository questionRepository, 
                              AnswerHistoryLog answerHistoryLog)
        {
            _questionRepository = questionRepository;
            _answerHistoryLog = answerHistoryLog;
        }

        public AnswerQuestionResult Run(int questionId, string answer, int userId)
        {
            var question = _questionRepository.GetById(questionId);

            var result = new AnswerQuestionResult();
            result.IsCorrect = question.Solution == answer.Trim();
            result.CorrectAnswer = question.Solution;
            result.AnswerGiven = answer;

            _answerHistoryLog.Run(question, result, userId);

            return result;
        }
    }
}
