using System;

public class KnowledgeSummary
{
    public int NotLearned = 0;
    public int NotLearnedPercentage { get { return Percentage(NotLearned); } }

    public int NeedsLearning = 0;
    public int NeedsLearningPercentage { get { return Percentage(NeedsLearning); } }

    public int NeedsConsolidation = 0;
    public int NeedsConsolidationPercentage { get { return Percentage(NeedsConsolidation); } }

    public int Solid = 0;
    public int SolidPercentage { get { return Percentage(Solid); } }


    /// <summary>Sum of questions in wish knowledge</summary>
    public int Total{ get { return NotLearned + NeedsLearning + NeedsConsolidation + Solid; }}

    private int Percentage(int amount)
    {
        if (Total == 0 || amount == 0)
            return 0;

        return (int)Math.Round(amount / (decimal)Total * 100);
    }
}