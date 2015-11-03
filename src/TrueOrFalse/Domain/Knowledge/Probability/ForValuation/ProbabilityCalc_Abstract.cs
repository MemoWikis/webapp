using System.Collections.Generic;
using System.Linq;
using NHibernate;

public abstract class ProbabilityCalc_Abstract
{
    public ProbabilityCalcResult Run(Question question, User user)
    {
        var answers = Sl.R<AnswerRepo>().GetByQuestion(question.Id, user.Id);

        if(Sl.R<ISession>().Get<Question>(question.Id) == null || Sl.R<ISession>().Get<User>(user.Id) == null)
            return new ProbabilityCalcResult { Probability = 0, KnowledgeStatus = KnowledgeStatus.Unknown };

        return Run(answers, question, user);
    }

    public ProbabilityCalcResult Run(Question question, User user, IList<Answer> answers)
    {
        if(question == null || user == null)
            return new ProbabilityCalcResult {Probability = 0, KnowledgeStatus = KnowledgeStatus.Unknown};

        answers = answers
            .Where(x =>
                x.QuestionId == question.Id &&
                x.UserId == user.Id
            ).ToList();

        return Run(answers, question, user);
    }

	public abstract ProbabilityCalcResult Run(IList<Answer> previousHistoryItems, Question question, User user);
}