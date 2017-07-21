using System;
using System.Collections.Generic;
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
        NotInWishknowledge = notInWishKnowledge;
        NotLearned = notLearned;
        NeedsLearning = needsLearning;
        NeedsConsolidation = needsConsolidation;
        Solid = solid;

        Percents.FromIntsd(new List<PercentAction>
        {
            new PercentAction{Value = NotLearned, Action = percent => NotLearnedPercentage = percent },
            new PercentAction{Value = NeedsLearning, Action = percent => NeedsLearningPercentage = percent },
            new PercentAction{Value = NeedsConsolidation, Action = percent => NeedsConsolidationPercentage = percent },
            new PercentAction{Value = Solid, Action = percent => SolidPercentage = percent },
            new PercentAction{Value = NotInWishknowledge, Action = percent => NotInWishknowledgePercentage = percent },
        });
    }

    private int Percentage(int amount)
    {
        if (Total == 0 || amount == 0)
            return 0;

        return (int)Math.Round(amount / (decimal)Total * 100);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}