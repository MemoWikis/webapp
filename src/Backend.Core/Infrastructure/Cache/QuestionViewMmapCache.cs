using MessagePack;

[MessagePackObject]
public record struct QuestionViewSummaryWithId(
    [property: Key(0)] Int64 Count,
    [property: Key(1)] DateTime DateOnly,
    [property: Key(2)] int QuestionId,
    [property: Key(3)] DateTime DateCreated);

public class QuestionViewMmapCache : MmapCacheBase<QuestionViewSummaryWithId>, IRegisterAsInstancePerLifetime
{
    public const int SchemaVersion = 1;

    protected override int SchemaVersionValue => SchemaVersion;

    public QuestionViewMmapCache() : base("questionviews.mmap", "QuestionView") { }

    public List<QuestionViewSummaryWithId> LoadQuestionViews() => Load();

    public void SaveAllQuestionViews(IList<QuestionViewSummaryWithId> views) => SaveAll(views);

    public void AppendQuestionView(QuestionViewSummaryWithId view) =>
        AppendSingle(view, $"QuestionId={view.QuestionId}, DateOnly={view.DateOnly}");

    public void AppendQuestionViews(IEnumerable<QuestionViewSummaryWithId> views) => AppendRange(views);

    public void DeleteQuestionViews(int questionId) =>
        DeleteWhere(v => v.QuestionId == questionId, $"QuestionId={questionId}");
}
