using System.Collections.Generic;
using System.Linq;

public abstract class ProbabilityCalc_Abstract
{
    public ProbabilityCalcResult Run(int questionId, int userId)
    {
        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetBy(questionId, userId);
        return Run(answerHistoryItems);
    }

    public ProbabilityCalcResult Run(int questionId, int userId, IList<AnswerHistory> answerHistoryItems)
    {
        answerHistoryItems = answerHistoryItems
            .Where(x =>
                x.QuestionId == questionId &&
                x.UserId == userId
            ).ToList();

        return Run(answerHistoryItems);
    }

	public abstract ProbabilityCalcResult Run(IList<AnswerHistory> answerHistoryItems);
}