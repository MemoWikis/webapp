using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class KnowledgeStatusCounts
{
    [JsonProperty("NotLearned")]
    public readonly int NotLearned = 0;
    
    [JsonProperty("NeedsLearning")]
    public readonly int NeedsLearning = 0;
    
    [JsonProperty("NeedsConsolidation")]
    public readonly int NeedsConsolidation = 0;
    
    [JsonProperty("Solid")]
    public readonly int Solid = 0;

    // Percentages relative to this KnowledgeStatusCounts total (adds up to 100% within this group)
    [JsonProperty("NotLearnedPercentage")]
    public int NotLearnedPercentage { get; private set; }
    
    [JsonProperty("NeedsLearningPercentage")]
    public int NeedsLearningPercentage { get; private set; }
    
    [JsonProperty("NeedsConsolidationPercentage")]
    public int NeedsConsolidationPercentage { get; private set; }
    
    [JsonProperty("SolidPercentage")]
    public int SolidPercentage { get; private set; }

    // Percentages relative to grand total (InWishknowledge + NotInWishknowledge)
    [JsonProperty("NotLearnedPercentageOfTotal")]
    public int NotLearnedPercentageOfTotal { get; private set; }
    
    [JsonProperty("NeedsLearningPercentageOfTotal")]
    public int NeedsLearningPercentageOfTotal { get; private set; }
    
    [JsonProperty("NeedsConsolidationPercentageOfTotal")]
    public int NeedsConsolidationPercentageOfTotal { get; private set; }
    
    [JsonProperty("SolidPercentageOfTotal")]
    public int SolidPercentageOfTotal { get; private set; }

    public KnowledgeStatusCounts(
        int notLearned = 0,
        int needsLearning = 0,
        int needsConsolidation = 0,
        int solid = 0)
    {
        NotLearned = notLearned;
        NeedsLearning = needsLearning;
        NeedsConsolidation = needsConsolidation;
        Solid = solid;

        CalculatePercentages();
    }

    private void CalculatePercentages()
    {
        PercentageShares.FromAbsoluteShares(new List<ValueWithResultAction>
        {
            new ValueWithResultAction
            {
                AbsoluteValue = NotLearned, ActionForPercentage = percent => NotLearnedPercentage = percent
            },
            new ValueWithResultAction
            {
                AbsoluteValue = NeedsLearning, ActionForPercentage = percent => NeedsLearningPercentage = percent
            },
            new ValueWithResultAction
            {
                AbsoluteValue = NeedsConsolidation,
                ActionForPercentage = percent => NeedsConsolidationPercentage = percent
            },
            new ValueWithResultAction
            {
                AbsoluteValue = Solid, ActionForPercentage = percent => SolidPercentage = percent
            },
        });
    }

    public void CalculatePercentagesOfTotal(int grandTotal)
    {
        if (grandTotal == 0)
        {
            NotLearnedPercentageOfTotal = 0;
            NeedsLearningPercentageOfTotal = 0;
            NeedsConsolidationPercentageOfTotal = 0;
            SolidPercentageOfTotal = 0;
            return;
        }

        NotLearnedPercentageOfTotal = (int)Math.Round((double)NotLearned / grandTotal * 100);
        NeedsLearningPercentageOfTotal = (int)Math.Round((double)NeedsLearning / grandTotal * 100);
        NeedsConsolidationPercentageOfTotal = (int)Math.Round((double)NeedsConsolidation / grandTotal * 100);
        SolidPercentageOfTotal = (int)Math.Round((double)Solid / grandTotal * 100);
    }

    [JsonProperty("Total")]
    public int Total => NotLearned + NeedsLearning + NeedsConsolidation + Solid;
}

[JsonObject(MemberSerialization.OptIn)]
public class KnowledgeSummary
{

    [JsonProperty("NotInWishknowledgePercentage")]
    public int NotInWishknowledgePercentage
    {
        get
        {
            if (TotalCount == 0) return 0;
            return (int)Math.Round((double)NotInWishknowledge.Total / TotalCount * 100);
        }
    }

    // New nested structure
    [JsonProperty("InWishknowledge")]
    public readonly KnowledgeStatusCounts InWishknowledge;
    
    [JsonProperty("NotInWishknowledge")]
    public readonly KnowledgeStatusCounts NotInWishknowledge;

    // Total of InWishknowledge + NotInWishknowledge
    [JsonProperty("Total")]
    public readonly KnowledgeStatusCounts Total;

    /// <summary>Sum of all questions (including those not in wish knowledge)</summary>
    public int TotalCount => InWishknowledge.Total + NotInWishknowledge.Total;

    /// <summary>
    /// Knowledge status points for questions in wishknowledge only, calculated as: 
    /// ((solidKnowledge * 1) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / (solidKnowledge + needsConsolidation + needsLearning + notLearnedInWishknowledge)
    /// </summary>
    [JsonProperty("KnowledgeStatusPoints")]
    public double KnowledgeStatusPoints { get; private set; }

    /// <summary>
    /// Knowledge status points for all questions including those not in wishknowledge, calculated as: 
    /// ((solidKnowledge * 1) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / (solidKnowledge + needsConsolidation + needsLearning + notLearned + notInWishknowledge)
    /// </summary>
    [JsonProperty("KnowledgeStatusPointsTotal")]
    public double KnowledgeStatusPointsTotal { get; private set; }

    public KnowledgeSummary(
        int notLearnedInWishknowledge = 0,
        int needsLearningInWishknowledge = 0,
        int needsConsolidationInWishknowledge = 0,
        int solidInWishknowledge = 0,
        int notLearnedNotInWishknowledge = 0,
        int needsLearningNotInWishknowledge = 0,
        int needsConsolidationNotInWishknowledge = 0,
        int solidNotInWishknowledge = 0)
    {
        InWishknowledge = new KnowledgeStatusCounts(
            notLearnedInWishknowledge,
            needsLearningInWishknowledge,
            needsConsolidationInWishknowledge,
            solidInWishknowledge);

        NotInWishknowledge = new KnowledgeStatusCounts(
            notLearnedNotInWishknowledge,
            needsLearningNotInWishknowledge,
            needsConsolidationNotInWishknowledge,
            solidNotInWishknowledge);

        // Create Total with combined counts
        Total = new KnowledgeStatusCounts(
            notLearnedInWishknowledge + notLearnedNotInWishknowledge,
            needsLearningInWishknowledge + needsLearningNotInWishknowledge,
            needsConsolidationInWishknowledge + needsConsolidationNotInWishknowledge,
            solidInWishknowledge + solidNotInWishknowledge);

        // Calculate percentages relative to grand total for both groups
        var grandTotal = TotalCount;
        InWishknowledge.CalculatePercentagesOfTotal(grandTotal);
        NotInWishknowledge.CalculatePercentagesOfTotal(grandTotal);

        CalculateKnowledgeStatusPoints();
    }

    private void CalculateKnowledgeStatusPoints()
    {
        // Calculate weighted scores for both wishknowledge and total
        var weightedScoreInWishknowledge = (InWishknowledge.Solid * 1.0) + (InWishknowledge.NeedsConsolidation * 0.5) + (InWishknowledge.NeedsLearning * 0.1);
        var totalSolid = InWishknowledge.Solid + NotInWishknowledge.Solid;
        var totalNeedsConsolidation = InWishknowledge.NeedsConsolidation + NotInWishknowledge.NeedsConsolidation;
        var totalNeedsLearning = InWishknowledge.NeedsLearning + NotInWishknowledge.NeedsLearning;
        var weightedScoreTotal = (totalSolid * 1.0) + (totalNeedsConsolidation * 0.5) + (totalNeedsLearning * 0.1);

        // Calculate rating for wishknowledge questions only
        var wishknowledgeQuestions = InWishknowledge.Total;

        if (wishknowledgeQuestions == 0)
        {
            KnowledgeStatusPoints = 0;
        }
        else
        {
            // Add small baseline value to prioritize pages with questions over pages without questions
            KnowledgeStatusPoints = Math.Round((weightedScoreInWishknowledge / wishknowledgeQuestions) + 0.0001, 4);
        }

        // Calculate rating for all questions including those not in wishknowledge
        var totalQuestions = TotalCount;

        if (totalQuestions == 0)
        {
            KnowledgeStatusPointsTotal = 0;
        }
        else
        {
            // Add small baseline value to prioritize pages with questions over pages without questions
            KnowledgeStatusPointsTotal = Math.Round((weightedScoreTotal / totalQuestions) + 0.0001, 4);
        }
    }
}