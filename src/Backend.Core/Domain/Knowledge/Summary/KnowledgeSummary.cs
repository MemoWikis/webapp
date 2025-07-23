using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class KnowledgeSummary
{
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

    public KnowledgeSummary(int notInWishKnowledge = 0, int notLearned = 0, int needsLearning = 0, int needsConsolidation = 0, int solid = 0)
    {
        NotInWishknowledge = 0;
        NotLearned = notLearned + notInWishKnowledge;
        NeedsLearning = needsLearning;
        NeedsConsolidation = needsConsolidation;
        Solid = solid;

        PercentageShares.FromAbsoluteShares(new List<ValueWithResultAction>
        {
            new ValueWithResultAction{AbsoluteValue = NotLearned, ActionForPercentage = percent => NotLearnedPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = NeedsLearning, ActionForPercentage = percent => NeedsLearningPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = NeedsConsolidation, ActionForPercentage = percent => NeedsConsolidationPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = Solid, ActionForPercentage = percent => SolidPercentage = percent },
            new ValueWithResultAction{AbsoluteValue = NotInWishknowledge, ActionForPercentage = percent => NotInWishknowledgePercentage = percent },
        });
    }

    /// <summary>
    /// Get the overall skill level as a simple descriptor
    /// </summary>
    public string GetOverallSkillLevel()
    {
        if (Total == 0) return "No Data";
        
        var solidPercentage = (double)Solid / Total * 100;
        var consolidationPercentage = (double)NeedsConsolidation / Total * 100;
        
        if (solidPercentage >= 70) return "Expert";
        if (solidPercentage >= 40) return "Advanced";
        if (consolidationPercentage + solidPercentage >= 60) return "Intermediate";
        if (NeedsLearning > 0 || NeedsConsolidation > 0) return "Basic";
        return "Beginner";
    }
}


   