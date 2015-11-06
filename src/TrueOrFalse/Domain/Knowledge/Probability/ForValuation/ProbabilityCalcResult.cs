using System.Collections.Generic;

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

        if (answers.Count <= 4)
            result.KnowledgeStatus = KnowledgeStatus.Unknown;
        else
            result.KnowledgeStatus =
                correctnessProbability >= 89 ?
                    KnowledgeStatus.Secure :
                    KnowledgeStatus.Weak;

	    return result;
    }
}