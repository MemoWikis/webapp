using System.Text.Json.Serialization;


public class MeiliSearchUserMap
{
    public string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public int WishCountQuestions { get; set; }
    public int Rank { get; set; }
    public int Id { get; set; }

    [JsonPropertyName("ContentLanguages")]
    public List<string?> ContentLanguages { get; set; }
}