using System.Text.Json.Serialization;

public class MeilisearchPageMap
{
    public int Id { get; set; }
    public string CreatorName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("Language")]
    public string Language { get; set; }
}