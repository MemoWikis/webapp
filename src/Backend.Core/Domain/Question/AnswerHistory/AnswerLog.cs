﻿public class AnswerLog(AnswerRepo answerRepo, QuestionReadingRepo questionReadingRepo, LoggedInUserCache _loggedInUserCache)
    : IRegisterAsInstancePerLifetime
{
    public void Run(
        Question question,
        AnswerQuestionResult answerQuestionResult,
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        Guid learningSessionStepGuid = default(Guid),
        //  bool countUnansweredAsCorrect = false,
        /*for testing*/
        DateTime dateCreated = default(DateTime))
    {
        var answer = new Answer
        {
            Question = question,
            UserId = userId,
            QuestionViewGuid = questionViewGuid,
            InteractionNumber = interactionNumber,
            MillisecondsSinceQuestionView = millisecondsSinceQuestionView,
            AnswerText = answerQuestionResult.AnswerGiven,
            AnswerredCorrectly = answerQuestionResult.IsCorrect
                ? AnswerCorrectness.True
                : AnswerCorrectness.False,
            DateCreated = dateCreated == default(DateTime)
                ? DateTime.Now
                : dateCreated
        };

        answerRepo.Create(answer);
        AnswerCache.AddAnswerToCache(_loggedInUserCache, answer);
    }

    public void CountLastAnswerAsCorrect(Guid questionViewGuid)
    {
        var correctedAnswer = answerRepo
            .GetByQuestionViewGuid(questionViewGuid)
            .OrderBy(a => a.InteractionNumber)
            .LastOrDefault(a => a.AnswerredCorrectly == AnswerCorrectness.False);

        if (correctedAnswer != null &&
            correctedAnswer.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            answerRepo.Update(correctedAnswer);
        }
    }

    public void CountUnansweredAsCorrect(
        Question question,
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView)
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
        };

        answerRepo.Create(answer);
    }

    public void LogAnswerView(
        QuestionCacheItem question,
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        int? roundId = null,
        int LearningSessionId = -1,
        Guid LearningSessionStepGuid = default(Guid))
    {
        var answer = new Answer
        {
            Question = questionReadingRepo.GetById(question.Id),
            UserId = userId,
            QuestionViewGuid = questionViewGuid,
            InteractionNumber = interactionNumber,
            MillisecondsSinceQuestionView = millisecondsSinceQuestionView,
            AnswerText = "",
            AnswerredCorrectly = AnswerCorrectness.IsView,
            DateCreated = DateTime.Now,
        };

        answerRepo.Create(answer);
    }
}