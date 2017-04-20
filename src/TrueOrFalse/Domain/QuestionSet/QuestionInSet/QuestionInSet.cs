using System;
using Seedworks.Lib.Persistence;

/// <summary>
/// Relation between QuestionSet -> Question
/// Contains order
/// </summary>
[Serializable]
public class QuestionInSet : DomainEntity
{
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }
    public virtual int Sort { get; set; }

    /// <summary>Time in seconds</summary>
    public virtual int Timecode { get; set; }

    public virtual string FormatedTimecode()
    {
        var timeSpan = new TimeSpan(0, 0, 0, this.Timecode);
        var dateTime = new DateTime(timeSpan.Ticks);

        return dateTime.ToString(timeSpan.Hours < 1 ? "mm:ss" : "HH:mm:ss");
    }
}