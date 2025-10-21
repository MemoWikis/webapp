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

    // Percentages relative to grand total (InWishKnowledge + NotInWishKnowledge)
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

    [JsonProperty("NotInWishKnowledgePercentage")]
    public int NotInWishKnowledgePercentage
    {
        get
        {
            if (TotalCount == 0) return 0;
            return (int)Math.Round((double)NotInWishKnowledge.Total / TotalCount * 100);
        }
    }

    // New nested structure
    [JsonProperty("InWishKnowledge")]
    public readonly KnowledgeStatusCounts InWishKnowledge;
    
    [JsonProperty("NotInWishKnowledge")]
    public readonly KnowledgeStatusCounts NotInWishKnowledge;

    // Total of InWishKnowledge + NotInWishKnowledge
    [JsonProperty("Total")]
    public readonly KnowledgeStatusCounts Total;

    /// <summary>Sum of all questions (including those not in wish knowledge)</summary>
    public int TotalCount => InWishKnowledge.Total + NotInWishKnowledge.Total;

    /// <summary>
    /// Knowledge status points for questions in wishknowledge only, calculated as: 
    /// ((solidKnowledge * 1) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / (solidKnowledge + needsConsolidation + needsLearning + notLearnedInWishKnowledge)
    /// </summary>
    [JsonProperty("KnowledgeStatusPoints")]
    public double KnowledgeStatusPoints { get; private set; }

    /// <summary>
    /// Knowledge status points for all questions including those not in wishknowledge, calculated as: 
    /// ((solidKnowledge * 1) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / (solidKnowledge + needsConsolidation + needsLearning + notLearned + notInWishKnowledge)
    /// </summary>
    [JsonProperty("KnowledgeStatusPointsTotal")]
    public double KnowledgeStatusPointsTotal { get; private set; }

    public KnowledgeSummary(
        int notLearnedInWishKnowledge = 0,
        int needsLearningInWishKnowledge = 0,
        int needsConsolidationInWishKnowledge = 0,
        int solidInWishKnowledge = 0,
        int notLearnedNotInWishKnowledge = 0,
        int needsLearningNotInWishKnowledge = 0,
        int needsConsolidationNotInWishKnowledge = 0,
        int solidNotInWishKnowledge = 0)
    {
        InWishKnowledge = new KnowledgeStatusCounts(
            notLearnedInWishKnowledge,
            needsLearningInWishKnowledge,
            needsConsolidationInWishKnowledge,
            solidInWishKnowledge);

        NotInWishKnowledge = new KnowledgeStatusCounts(
            notLearnedNotInWishKnowledge,
            needsLearningNotInWishKnowledge,
            needsConsolidationNotInWishKnowledge,
            solidNotInWishKnowledge);

        // Create Total with combined counts
        Total = new KnowledgeStatusCounts(
            notLearnedInWishKnowledge + notLearnedNotInWishKnowledge,
            needsLearningInWishKnowledge + needsLearningNotInWishKnowledge,
            needsConsolidationInWishKnowledge + needsConsolidationNotInWishKnowledge,
            solidInWishKnowledge + solidNotInWishKnowledge);

        // Calculate percentages relative to grand total for both groups
        var grandTotal = TotalCount;
        InWishKnowledge.CalculatePercentagesOfTotal(grandTotal);
        NotInWishKnowledge.CalculatePercentagesOfTotal(grandTotal);

        CalculateKnowledgeStatusPoints();
    }

    private void CalculateKnowledgeStatusPoints()
    {
        // Calculate weighted scores for both wishknowledge and total
        var weightedScoreInWishKnowledge = (InWishKnowledge.Solid * 1.0) + (InWishKnowledge.NeedsConsolidation * 0.5) + (InWishKnowledge.NeedsLearning * 0.1);
        var totalSolid = InWishKnowledge.Solid + NotInWishKnowledge.Solid;
        var totalNeedsConsolidation = InWishKnowledge.NeedsConsolidation + NotInWishKnowledge.NeedsConsolidation;
        var totalNeedsLearning = InWishKnowledge.NeedsLearning + NotInWishKnowledge.NeedsLearning;
        var weightedScoreTotal = (totalSolid * 1.0) + (totalNeedsConsolidation * 0.5) + (totalNeedsLearning * 0.1);

        // Calculate rating for wishknowledge questions only
        var wishknowledgeQuestions = InWishKnowledge.Total;

        if (wishknowledgeQuestions == 0)
        {
            KnowledgeStatusPoints = 0;
        }
        else
        {
            // Add small baseline value to prioritize pages with questions over pages without questions
            KnowledgeStatusPoints = Math.Round((weightedScoreInWishKnowledge / wishknowledgeQuestions) + 0.0001, 4);
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