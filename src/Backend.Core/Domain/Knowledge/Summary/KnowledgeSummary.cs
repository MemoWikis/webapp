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

    // Aggregated not-in-wish-knowledge data for Total object
    [JsonProperty("NotInWishKnowledgeCount")]
    public int? NotInWishKnowledgeCount { get; private set; }
    
    [JsonProperty("NotInWishKnowledgePercentage")]
    public int? NotInWishKnowledgePercentage { get; private set; }

    public KnowledgeStatusCounts(
        int notLearned = 0,
        int needsLearning = 0,
        int needsConsolidation = 0,
        int solid = 0,
        int? notInWishKnowledgeCount = null,
        int? notInWishKnowledgePercentage = null)
    {
        NotLearned = notLearned;
        NeedsLearning = needsLearning;
        NeedsConsolidation = needsConsolidation;
        Solid = solid;
        NotInWishKnowledgeCount = notInWishKnowledgeCount;
        NotInWishKnowledgePercentage = notInWishKnowledgePercentage;

        CalculatePercentages();
    }

    private void CalculatePercentages()
    {
        var valueWithResultActions = new List<ValueWithResultAction>
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
            }
        };

        // Only include NotInWishKnowledge percentage calculation if the count is not null
        if (NotInWishKnowledgeCount.HasValue)
        {
            valueWithResultActions.Add(new ValueWithResultAction
            {
                AbsoluteValue = NotInWishKnowledgeCount.Value, 
                ActionForPercentage = percent => NotInWishKnowledgePercentage = percent
            });
        }

        PercentageShares.FromAbsoluteShares(valueWithResultActions);
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

    [JsonProperty("InWishKnowledge")]
    public readonly KnowledgeStatusCounts InWishKnowledge;
    
    [JsonProperty("NotInWishKnowledge")]
    public readonly KnowledgeStatusCounts NotInWishKnowledge;

    [JsonProperty("Total")]
    public readonly KnowledgeStatusCounts Total;

    /// <summary>Sum of all questions (including those not in wish knowledge)</summary>
    public int TotalCount => InWishKnowledge.Total + NotInWishKnowledge.Total;

    /// <summary>
    /// Knowledge status points for questions in wishKnowledge only, calculated as: 
    /// ((solidKnowledge * 1) + (needsConsolidation * 0.5) + (needsLearning * 0.1)) / (solidKnowledge + needsConsolidation + needsLearning + notLearnedInWishKnowledge)
    /// </summary>
    [JsonProperty("KnowledgeStatusPoints")]
    public double KnowledgeStatusPoints { get; private set; }

    /// <summary>
    /// Knowledge status points for all questions including those not in wishKnowledge, calculated as: 
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
            solidInWishKnowledge,
            notInWishKnowledgeCount: null,
            notInWishKnowledgePercentage: null);

        NotInWishKnowledge = new KnowledgeStatusCounts(
            notLearnedNotInWishKnowledge,
            needsLearningNotInWishKnowledge,
            needsConsolidationNotInWishKnowledge,
            solidNotInWishKnowledge,
            notInWishKnowledgeCount: null,
            notInWishKnowledgePercentage: null);

        var notInWishTotal = notLearnedNotInWishKnowledge + needsLearningNotInWishKnowledge + needsConsolidationNotInWishKnowledge + solidNotInWishKnowledge;

        Total = new KnowledgeStatusCounts(
            notLearnedInWishKnowledge,
            needsLearningInWishKnowledge,
            needsConsolidationInWishKnowledge,
            solidInWishKnowledge,
            notInWishKnowledgeCount: notInWishTotal,
            notInWishKnowledgePercentage: null);

        var grandTotal = TotalCount;
        InWishKnowledge.CalculatePercentagesOfTotal(grandTotal);
        NotInWishKnowledge.CalculatePercentagesOfTotal(grandTotal);

        CalculateKnowledgeStatusPoints();
    }

    private void CalculateKnowledgeStatusPoints()
    {
        var weightedScoreInWishKnowledge = (InWishKnowledge.Solid * 1.0) + (InWishKnowledge.NeedsConsolidation * 0.5) + (InWishKnowledge.NeedsLearning * 0.1);
        var totalSolid = InWishKnowledge.Solid + NotInWishKnowledge.Solid;
        var totalNeedsConsolidation = InWishKnowledge.NeedsConsolidation + NotInWishKnowledge.NeedsConsolidation;
        var totalNeedsLearning = InWishKnowledge.NeedsLearning + NotInWishKnowledge.NeedsLearning;
        var weightedScoreTotal = (totalSolid * 1.0) + (totalNeedsConsolidation * 0.5) + (totalNeedsLearning * 0.1);

        var wishKnowledgeQuestions = InWishKnowledge.Total;

        if (wishKnowledgeQuestions == 0)
        {
            KnowledgeStatusPoints = 0;
        }
        else
        {
            KnowledgeStatusPoints = Math.Round((weightedScoreInWishKnowledge / wishKnowledgeQuestions) + 0.0001, 4);
        }

        var totalQuestions = TotalCount;

        if (totalQuestions == 0)
        {
            KnowledgeStatusPointsTotal = 0;
        }
        else
        {
            KnowledgeStatusPointsTotal = Math.Round((weightedScoreTotal / totalQuestions) + 0.0001, 4);
        }
    }
}