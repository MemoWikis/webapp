using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[JsonObject(MemberSerialization.OptIn)]
public class TrainingQuestion
{
    private Question _question;

    public Question Question
    {
        get
        {
            if (_question != null)
                return _question;

            _question = Sl.R<QuestionRepo>().GetById(QuestionId);
            QuestionId= _question.Id;
            return _question;
        }
        set
        {
            _question = value;
            QuestionId = _question.Id;
        }
    }
    
    [JsonProperty]
    public int QuestionId { get; set; }

    /// <summary>Probability Before</summary>
    [JsonProperty]
    public int ProbBefore { get; set; }

    /// <summary>Probability After</summary>
    [JsonProperty]
    public int ProbAfter { get; set; }

    [JsonProperty]
    public bool IsInTraining { get; set; }
}