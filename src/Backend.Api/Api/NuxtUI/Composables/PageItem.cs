public readonly record struct PageItem(
    int Id = 0,
    string Name = "",
    string ImgUrl = "",
    int? QuestionCount = null,
    KnowledgeSummaryResponse KnowledgebarData = new KnowledgeSummaryResponse(),
    int? Popularity = null);