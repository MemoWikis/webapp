using System;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class KnowledgeSummary
{
    [JsonProperty("NotLearned")]
    public int NotLearned = 0;
    public int NotLearnedPercentage => Percentage(NotLearned);

    [JsonProperty("NeedsLearning")]
    public int NeedsLearning = 0;
    public int NeedsLearningPercentage => Percentage(NeedsLearning);

    [JsonProperty("NeedsConsolidation")]
    public int NeedsConsolidation = 0;
    public int NeedsConsolidationPercentage => Percentage(NeedsConsolidation);

    [JsonProperty("Solid")]
    public int Solid = 0;
    public int SolidPercentage => Percentage(Solid);


    /// <summary>Sum of questions in wish knowledge</summary>
    public int Total => NotLearned + NeedsLearning + NeedsConsolidation + Solid;

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