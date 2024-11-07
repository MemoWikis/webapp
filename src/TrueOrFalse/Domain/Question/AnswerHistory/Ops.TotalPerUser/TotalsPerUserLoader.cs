public class TotalsPerUserLoader(AnswerRepo _answerRepo) : IRegisterAsInstancePerLifetime
{
    public TotalPerUser Run(SessionUser sessionUser, int questionId)
    {
        if (sessionUser.IsLoggedIn)
        {
            if (sessionUser.User.AnswerCounter.Count > 0 &&
                sessionUser.User.AnswerCounter.TryGetValue(questionId, out var answersByUser))
                return GetTotalPerUser(answersByUser, questionId);

        }
        else if (!sessionUser.IsLoggedIn)
        {
            var answerCounter = GetAnswerRecord(questionId);

            return GetTotalPerUser(answerCounter, questionId);
        }

        return new TotalPerUser();
    }

    private TotalPerUser GetTotalPerUser(AnswerRecord answerCounter, int questionId)
    {
        var totalTrue = answerCounter.True + answerCounter.MarkedAsTrue;
        var totalFalse = answerCounter.False;
        return new TotalPerUser
        {
            QuestionId = questionId,
            TotalTrue = totalTrue,
            TotalFalse = totalFalse
        };
    }

    private AnswerRecord GetAnswerRecord(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        if (question.AnswerCounter.True == 0 &&
            question.AnswerCounter.False == 0 &&
            question.AnswerCounter.MarkedAsTrue == 0 &&
            question.AnswerCounter.View == 0)
        {
            var answers = _answerRepo.GetByQuestion(questionId);
            question.AnswerCounter = AnswerCache.AnswersToAnswerRecord(answers);
            EntityCache.AddOrUpdate(question);
        }

        return question.AnswerCounter;
    }
}