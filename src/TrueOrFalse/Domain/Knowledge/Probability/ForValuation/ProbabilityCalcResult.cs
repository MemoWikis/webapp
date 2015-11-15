using System;
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

        // Definition KnowledgeStatus:
        //   NotLearned = no answer existing
        //   NeedsLearning = Last answer wrong OR among last five answers: wrong >= correct
        //   NeedsConsolidation = residual category
        //   Solid = Last three answers correct AND c<4b
        //   Solid (old) = Last three answers correct AND ((no wrong answer existing AND c<4b) OR (b > c-a))
        //      where:  a = Distance between last incorrect answer and beginning of positive streak
        //              b = Length of last positive streak
        //              c = Distance from end of last posivite streak to now
        //      example (o=correct; x=wrong answer; |=now): ... x   o  o o   o        o     |
        //                                                       -a- ---------b------- --c--

        if (answers.Count == 0) {
            result.KnowledgeStatus = KnowledgeStatus.NotLearned;
            return result;
        }
        if (!answers.Last().AnsweredCorrectly())
        {
            result.KnowledgeStatus = KnowledgeStatus.NeedsLearning;
            return result;
        }

        var lastFiveAnswers = answers.OrderByDescending(a => a.DateCreated).Take(5).ToList();
        if (lastFiveAnswers.Count(a => !a.AnsweredCorrectly()) >=
            lastFiveAnswers.Count(a => a.AnsweredCorrectly()))
        {
            result.KnowledgeStatus = KnowledgeStatus.NeedsLearning;
            return result;
        }

        var lastStreak = answers.OrderByDescending(a => a.DateCreated).TakeWhile(a => a.AnsweredCorrectly()).ToList();
        //var tmp_lengthStreak = lastStreak.First().DateCreated - lastStreak.Last().DateCreated;
        //var tmp_DistanceSinceLastStreak = DateTime.Now - lastStreak.First().DateCreated;
        //var tmp_4Times = TimeSpan.FromTicks(tmp_DistanceSinceLastStreak.Ticks*4);

        if ((lastStreak.Count >= 3) &&
            (DateTime.Now - lastStreak.First().DateCreated) <
            TimeSpan.FromTicks((lastStreak.First().DateCreated - lastStreak.Last().DateCreated).Ticks*4)) 
        {
            result.KnowledgeStatus = KnowledgeStatus.Solid;
            return result;
        }
        result.KnowledgeStatus = KnowledgeStatus.NeedsConsolidation;

	    return result;
    }
}