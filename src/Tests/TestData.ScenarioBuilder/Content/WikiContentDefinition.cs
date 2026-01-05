/// <summary>
/// Represents a complete wiki with subpages and questions from JSON content files.
/// </summary>
public sealed record WikiContentDefinition
{
    public string Title { get; init; } = "";
    public string Theme { get; init; } = "";
    public int Difficulty { get; init; } = 3;
    public PageContentDefinition MainPage { get; init; } = new();
    public List<PageContentDefinition> Subpages { get; init; } = new();
    public List<QuestionContentDefinition> Questions { get; init; } = new();
    public ContentMetadata Metadata { get; init; } = new();
}

/// <summary>
/// Represents a single page (main page or subpage).
/// </summary>
public sealed record PageContentDefinition
{
    public string Title { get; init; } = "";
    public string Content { get; init; } = "";
}

/// <summary>
/// Represents a question for a page.
/// </summary>
public sealed record QuestionContentDefinition
{
    public string PageTitle { get; init; } = "";
    public string Text { get; init; } = "";
    public string Solution { get; init; } = "";
    public string SolutionType { get; init; } = "Text";
    public bool IsCaseSensitive { get; init; } = false;
}

/// <summary>
/// Metadata about when and how the content was generated.
/// </summary>
public sealed record ContentMetadata
{
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
    public string AiModel { get; init; } = "";
    public List<string> Sources { get; init; } = new();
}
