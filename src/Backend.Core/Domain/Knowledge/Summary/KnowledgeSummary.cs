using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class KnowledgeSummary
{
    public string Options = "standard";

    [JsonProperty("NotLearned")]
    public readonly int NotLearned = 0;

    public int NotLearnedPercentage { get; private set; }

    [JsonProperty("NeedsLearning")]
    public readonly int NeedsLearning = 0;

    public int NeedsLearningPercentage { get; private set; }

    [JsonProperty("NeedsConsolidation")]
    public readonly int NeedsConsolidation = 0;
    public int NeedsConsolidationPercentage { get; private set; }

    [JsonProperty("Solid")]
    public readonly int Solid = 0;
    public int SolidPercentage { get; private set; }

    public readonly int NotInWishknowledge = 0;
    public int NotInWishknowledgePercentage { get; private set; }

    /// <summary>Sum of questions in wish knowledge</summary>
    public int Total => NotLearned + NeedsLearning + NeedsConsolidation + Solid + NotInWishknowledge;

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

    public KnowledgeSummary(int notInWishKnowledge = 0, int notLearned = 0, int needsLearning = 0, int needsConsolidation = 0, int solid = 0, string options = "standard")
    {
        NotInWishknowledge = 0;
        NotLearned = notLearned + notInWishKnowledge;
        NeedsLearning = needsLearning;
        NeedsConsolidation = needsConsolidation;
        Solid = solid;
        Options = options;

        PercentageShares.FromAbsoluteShares(new List<ValueWithResultAction>
        {
            new ValueWithResultAction{AbsoluteValue = NotLearned, ActionForPercentage = percent => NotLearnedPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = NeedsLearning, ActionForPercentage = percent => NeedsLearningPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = NeedsConsolidation, ActionForPercentage = percent => NeedsConsolidationPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = Solid, ActionForPercentage = percent => SolidPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = NotInWishknowledge, ActionForPercentage = percent => NotInWishknowledgePercentage = percent },
        });

        CalculateKnowledgeStatusPoints();
    }

    private void CalculateKnowledgeStatusPoints()
    {
        var weightedScore = (Solid * 1.0) + (NeedsConsolidation * 0.5) + (NeedsLearning * 0.1);

        // Calculate rating for wishknowledge questions only
        var wishknowledgeQuestions = Solid + NeedsConsolidation + NeedsLearning + (NotLearned - NotInWishknowledge);

        if (wishknowledgeQuestions == 0)
        {
            KnowledgeStatusPoints = 0;
        }
        else
        {
            // Add small baseline value to prioritize pages with questions over pages without questions
            KnowledgeStatusPoints = Math.Round((weightedScore / wishknowledgeQuestions) + 0.0001, 4);
        }

        // Calculate rating for all questions including those not in wishknowledge
        var totalQuestions = Solid + NeedsConsolidation + NeedsLearning + NotLearned;

        if (totalQuestions == 0)
        {
            KnowledgeStatusPointsTotal = 0;
        }
        else
        {
            // Add small baseline value to prioritize pages with questions over pages without questions
            KnowledgeStatusPointsTotal = Math.Round((weightedScore / totalQuestions) + 0.0001, 4);
        }
    }
}


