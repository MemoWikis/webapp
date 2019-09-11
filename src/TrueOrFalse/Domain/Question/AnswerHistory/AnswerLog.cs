using System;
using System.Linq;

public class AnswerLog : IRegisterAsInstancePerLifetime
{
    private readonly AnswerRepo _answerRepo;

    public AnswerLog(AnswerRepo answerRepo)
    {
        _answerRepo = answerRepo;
    }

    public void Run(
        Question question, 
        AnswerQuestionResult answerQuestionResult, 
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        Player player = null,
        Round round = null,
        LearningSession learningSession = null,
        Guid learningSessionStepGuid = default(Guid),
      //  bool countUnansweredAsCorrect = false,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        var answer = new Answer
        {
            Question = question,
            UserId = userId,
            QuestionViewGuid = questionViewGuid,
            InteractionNumber = interactionNumber,
            MillisecondsSinceQuestionView = millisecondsSinceQuestionView,
            AnswerText = answerQuestionResult.AnswerGiven,
            AnswerredCorrectly = answerQuestionResult.IsCorrect ? AnswerCorrectness.True : AnswerCorrectness.False,
            Round = round,
            Player = player,
            LearningSession = learningSession,
            LearningSessionStepGuid = learningSessionStepGuid,
            DateCreated = dateCreated == default(DateTime)
                ? DateTime.Now
                : dateCreated
        };

        _answerRepo.Create(answer);
    }

    public void CountLastAnswerAsCorrect(Guid questionViewGuid, int questionId)
    {
        var correctedAnswer = _answerRepo.GetByQuestion(questionId).Where(a => a.QuestionViewGuid == questionViewGuid).
            LastOrDefault(a => a.AnswerredCorrectly == AnswerCorrectness.False);
        if (correctedAnswer != null && correctedAnswer.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            _answerRepo.Update(correctedAnswer);
        }
    }

    public void CountUnansweredAsCorrect(Question question, int userId, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView, Guid learningSessionStepGuid, int? learningSessionId=-1)
    {
        var answer = new Answer
        {
            QuestionViewGuid = questionViewGuid,
            InteractionNumber = interactionNumber,
            MillisecondsSinceQuestionView = millisecondsSinceQuestionView,
            Question = question,
            UserId = userId,
            AnswerText = "",
            AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue,
            DateCreated = DateTime.Now,
            LearningSessionStepGuid = learningSessionStepGuid,
            LearningSession = learningSessionId !=-1 ? Sl.R<LearningSessionRepo>().GetById((int)learningSessionId) : null


        };

        _answerRepo.Create(answer);
    }

    public void LogAnswerView(Question question, int userId, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView, int? roundId = null, int LearningSessionId = -1, Guid LearningSessionStepGuid = default(Guid))
    {

        var answer = new Answer
        {
            Question = question,
            UserId = userId,
            QuestionViewGuid = questionViewGuid,
            InteractionNumber = interactionNumber,
            MillisecondsSinceQuestionView = millisecondsSinceQuestionView,
            AnswerText = "",
            AnswerredCorrectly = AnswerCorrectness.IsView,
            DateCreated = DateTime.Now,
            LearningSession = LearningSessionId != -1 ? Sl.R<LearningSessionRepo>().GetById(LearningSessionId) : null,
            LearningSessionStepGuid = LearningSessionStepGuid
        };

        if (roundId != null)
            answer.Round = Sl.R<RoundRepo>().GetById((int) roundId);

        _answerRepo.Create(answer);
    }
}
