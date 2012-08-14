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
        private readonly UpdateQuestionAnswerCount _updateQuestionAnswerCount;
        private readonly RecalculateKnowledgeItem _recalculateKnowledgeItem;

        public AnswerQuestion(QuestionRepository questionRepository, 
                              AnswerHistoryLog answerHistoryLog, 
                              UpdateQuestionAnswerCount updateQuestionAnswerCount, 
                              RecalculateKnowledgeItem recalculateKnowledgeItem)
        {
            _questionRepository = questionRepository;
            _answerHistoryLog = answerHistoryLog;
            _updateQuestionAnswerCount = updateQuestionAnswerCount;
            _recalculateKnowledgeItem = recalculateKnowledgeItem;
        }

        public AnswerQuestionResult Run(int questionId, string answer, int userId)
        {
            var question = _questionRepository.GetById(questionId);
            var solution = new GetQuestionSolution().Run(question.SolutionType, question.Solution);

            var result = new AnswerQuestionResult();
            result.IsCorrect = solution.IsCorrect(answer.Trim());
            result.CorrectAnswer = solution.CorrectAnswer();
            result.AnswerGiven = answer;

            _answerHistoryLog.Run(question, result, userId);
            _updateQuestionAnswerCount.Run(questionId, result.IsCorrect);
            _recalculateKnowledgeItem.Run(questionId, userId);

            return result;
        }
    }
}
