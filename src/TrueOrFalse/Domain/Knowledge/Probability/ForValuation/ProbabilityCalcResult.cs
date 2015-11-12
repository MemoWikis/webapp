using System.Collections.Generic;
using System.Linq;

public class ProbabilityCalcResult
{
    /// <summary>CorrectnessProbability</summary>
	public int Probability;
    public int AnswerCount;
    public KnowledgeStatus KnowledgeStatus;

    public static ProbabilityCalcResult GetResult(
        IList<Answer> answers, 
        int correctnessProbability)
    {
	    var result = new ProbabilityCalcResult
	    {
	        Probability = correctnessProbability,
            AnswerCount = answers.Count
	    };

        if (answers.Count == 0)
            result.KnowledgeStatus = KnowledgeStatus.NotLearned;
        else if (!answers.Last().AnsweredCorrectly())
            result.KnowledgeStatus = KnowledgeStatus.NeedsLearning;
        //else if (answers.OrderByDescending(d => d.DateCreated).Take(5).OrderBy(a => a.AnsweredCorrectly()))
        //else if () //Take last five answers; more wrong than correct then NeedsLearning
        //else if ((StreakOf3) &&
        //         ((NeverIncorrect && DistanceFromLastStreakToToday/LengthOfStreak < 4) ||
        //          (LengthOfStreak > DistanceFromLastWrongToStreakBegin + DistanceFromLastStreakToToday)))
        //    result.KnowledgeStatus = KnowledgeStatus.Solid;
        else
            result.KnowledgeStatus = KnowledgeStatus.NeedsConsolidation;
        
        //else
        //    result.KnowledgeStatus =
        //        correctnessProbability >= 89 ?
        //            KnowledgeStatus.Secure :
        //            KnowledgeStatus.Weak;

	    return result;
    }
}