using System.Collections.Generic;
using System.Linq;
using NHibernate;

public abstract class ProbabilityCalc_Abstract
{
    public ProbabilityCalcResult Run(Question question, User user)
    {
        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetByQuestion(question.Id, user.Id);

        if(Sl.R<ISession>().Get<Question>(question.Id) == null || Sl.R<ISession>().Get<User>(user.Id) == null)
            return new ProbabilityCalcResult { Probability = 0, KnowledgeStatus = KnowledgeStatus.Unknown };

        return Run(answerHistoryItems, question, user);
    }

    public ProbabilityCalcResult Run(Question question, User user, IList<AnswerHistory> answerHistoryItems)
    {
        if(question == null || user == null)
            return new ProbabilityCalcResult {Probability = 0, KnowledgeStatus = KnowledgeStatus.Unknown};

        answerHistoryItems = answerHistoryItems
            .Where(x =>
                x.QuestionId == question.Id &&
                x.UserId == user.Id
            ).ToList();

        return Run(answerHistoryItems, question, user);
    }

	public abstract ProbabilityCalcResult Run(IList<AnswerHistory> answerHistoryItems, Question question, User user);
}