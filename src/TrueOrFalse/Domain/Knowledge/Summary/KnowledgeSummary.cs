using System;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class KnowledgeSummary
{
    [JsonProperty("NotLearned")]
    public readonly int NotLearned = 0;
    public int NotLearnedPercentage => Percentage(NotLearned);

    [JsonProperty("NeedsLearning")]
    public readonly int NeedsLearning = 0;
    public int NeedsLearningPercentage => Percentage(NeedsLearning);

    [JsonProperty("NeedsConsolidation")]
    public readonly int NeedsConsolidation = 0;
    public int NeedsConsolidationPercentage => Percentage(NeedsConsolidation);

    [JsonProperty("Solid")]
    public readonly int Solid = 0;
    public int SolidPercentage => Percentage(Solid);

    public readonly int NotInWishknowledge = 0;
    public int NotInWishknowledgePercentage => Percentage(NotInWishknowledge);

    /// <summary>Sum of questions in wish knowledge</summary>
    public int Total => NotLearned + NeedsLearning + NeedsConsolidation + Solid + NotInWishknowledge;

    public KnowledgeSummary(int notInWishKnowledge = 0, int notLearned = 0, int needsLearning = 0, int needsConsolidation = 0, int solid = 0)
    {
        NotInWishknowledge = notInWishKnowledge;
        NotLearned = notLearned;
        NeedsLearning = needsLearning;
        NeedsConsolidation = needsConsolidation;
        Solid = solid;
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