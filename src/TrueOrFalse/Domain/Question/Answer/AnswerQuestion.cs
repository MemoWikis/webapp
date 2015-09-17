using System;
using TrueOrFalse;

public class AnswerQuestion : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepo _questionRepo;
    private readonly AnswerHistoryLog _answerHistoryLog;
    private readonly LearningSessionStepRepo _learningSessionStepRepo;

    public AnswerQuestion(QuestionRepo questionRepo, 
                            AnswerHistoryLog answerHistoryLog, 
                            LearningSessionStepRepo learningSessionStepRepo)
    {
        _questionRepo = questionRepo;
        _answerHistoryLog = answerHistoryLog;
        _learningSessionStepRepo = learningSessionStepRepo;
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

    public AnswerQuestionResult Run(
       int questionId,
       string answer,
       int userId,
       int stepId,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        var learningSessionStep = _learningSessionStepRepo.GetById(stepId);

        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerHistoryLog.Run(question, answerQuestionResult, userId, learningSessionStep: learningSessionStep, dateCreated: dateCreated);
            
            learningSessionStep.AnswerState = StepAnswerState.Answered;
            _learningSessionStepRepo.Update(learningSessionStep);

        });
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerHistoryLog.Run(question, answerQuestionResult, userId, dateCreated: dateCreated); 
        });
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Action<Question, AnswerQuestionResult> action)
    {
        var question = _questionRepo.GetById(questionId);
        var solution = new GetQuestionSolution().Run(question);

        var result = new AnswerQuestionResult();
        result.IsCorrect = solution.IsCorrect(answer.Trim());
        result.CorrectAnswer = solution.CorrectAnswer();
        result.AnswerGiven = answer;

        action(question, result);

        Sl.R<UpdateQuestionAnswerCount>().Run(questionId, result.IsCorrect);
        Sl.R<ProbabilityUpdate_Valuation>().Run(questionId, userId);
        ProbabilityUpdate_Question.Run(question);

        return result;
    }
}