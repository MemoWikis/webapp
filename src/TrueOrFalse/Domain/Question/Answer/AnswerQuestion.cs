using System;
using TrueOrFalse;

public class AnswerQuestion : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepository _questionRepository;
    private readonly AnswerHistoryLog _answerHistoryLog;
    private readonly UpdateQuestionAnswerCount _updateQuestionAnswerCount;
    private readonly ProbabilityForUserUpdate _probabilityForUserUpdate;

    public AnswerQuestion(QuestionRepository questionRepository, 
                            AnswerHistoryLog answerHistoryLog, 
                            UpdateQuestionAnswerCount updateQuestionAnswerCount, 
                            ProbabilityForUserUpdate probabilityForUserUpdate)
    {
        _questionRepository = questionRepository;
        _answerHistoryLog = answerHistoryLog;
        _updateQuestionAnswerCount = updateQuestionAnswerCount;
        _probabilityForUserUpdate = probabilityForUserUpdate;
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId, 
        int playerId,
        int roundId)
    {
        var player = Sl.R<PlayerRepo>().GetById(playerId);
        var round = Sl.R<RoundRepo>().GetById(roundId);

        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerHistoryLog.Run(question, answerQuestionResult, userId, player, round);
        });
    }

    public AnswerQuestionResult Run(int questionId, string answer, int userId)
    {
        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerHistoryLog.Run(question, answerQuestionResult, userId); 
        });
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Action<Question, AnswerQuestionResult> action)
    {
        var question = _questionRepository.GetById(questionId);
        var solution = new GetQuestionSolution().Run(question);

        var result = new AnswerQuestionResult();
        result.IsCorrect = solution.IsCorrect(answer.Trim());
        result.CorrectAnswer = solution.CorrectAnswer();
        result.AnswerGiven = answer;

        action(question, result);

        _updateQuestionAnswerCount.Run(questionId, result.IsCorrect);
        _probabilityForUserUpdate.Run(questionId, userId);

        return result;
    }
}