using System.Text.Json.Serialization;

public class MeiliSearchQuestionMap : IRegisterAsInstancePerLifetime
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }
    public string Solution { get; set; }
    public int SolutionType { get; set; }

    public int? CreatorId { get; set; }
    public ICollection<string> Pages { get; set; }
    public ICollection<int> PageIds { get; set; }

    [JsonPropertyName("Language")]
    public string Language { get; set; }
}