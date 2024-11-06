public class TotalsPerUserLoader : IRegisterAsInstancePerLifetime
{
    //public TotalPerUser Run(SessionUser sessionUser, int questionId)
    //{
    //    if (sessionUser.IsLoggedIn)
    //    {
    //        if (sessionUser.User.Answers.Count > 0 &&
    //            sessionUser.User.Answers.TryGetValue(questionId, out var answersByUser))
    //            return GetTotalPerUser(answersByUser, questionId);

    //    }
    //    else if (!sessionUser.IsLoggedIn)
    //    {
    //        var question = EntityCache.GetQuestionById(questionId);

    //        var answers = question.AnswersByAnonymousUsers;
    //        if (answers.Count > 0)
    //            return GetTotalPerUser(answers, questionId);

    //    }

    //    return new TotalPerUser();
    //}

    //private TotalPerUser GetTotalPerUser(List<AnswerCacheItem> answers, int questionId)
    //{
    //    var totalTrue = answers.Count(a => a.AnswerCorrectness == AnswerCorrectness.True || a.AnswerCorrectness == AnswerCorrectness.MarkedAsTrue);
    //    var totalFalse = answers.Count(a => a.AnswerCorrectness == AnswerCorrectness.False);

    //    return new TotalPerUser
    //    {
    //        QuestionId = questionId,
    //        TotalTrue = totalTrue,
    //        TotalFalse = totalFalse
    //    };
    //}

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
            var question = EntityCache.GetQuestionById(questionId);

            return GetTotalPerUser(question.AnswerCounter, questionId);
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
}