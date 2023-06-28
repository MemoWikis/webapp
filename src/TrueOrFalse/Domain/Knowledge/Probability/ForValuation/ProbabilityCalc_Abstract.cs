﻿using System.Collections.Generic;
using System.Linq;
using NHibernate;

public abstract class ProbabilityCalc_Abstract
{
    public ProbabilityCalcResult Run(QuestionCacheItem question, UserCacheItem user, ISession nhibernateSession)
    {
        var answers = Sl.R<AnswerRepo>().GetByQuestion(question.Id, user.Id);

        if(nhibernateSession.Get<Question>(question.Id) == null || nhibernateSession.Get<User>(user.Id) == null)
            return new ProbabilityCalcResult { Probability = 0, KnowledgeStatus = KnowledgeStatus.NotLearned };

        return Run(answers, question, user);
    }

    public abstract ProbabilityCalcResult Run(IList<Answer> previousHistoryItems, QuestionCacheItem question, UserCacheItem user);
}