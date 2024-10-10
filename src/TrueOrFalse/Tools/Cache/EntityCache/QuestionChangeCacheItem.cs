using Seedworks.Lib.Persistence;

[Serializable]
public class QuestionChangeCacheItem : IPersistable
{
    public virtual int Id { get; set; }

    private QuestionCacheItem? _questionCacheItem;
    public virtual QuestionCacheItem Question => _questionCacheItem ??= EntityCache.GetQuestion(QuestionId);
    public virtual int QuestionId { get; set; }
    public virtual string Data { get; set; }
    public virtual bool ShowInSidebar { get; set; } = true;

    public virtual int DataVersion { get; set; }

    public virtual int AuthorId { get; set; }

    public virtual UserCacheItem Author() => _author ??= EntityCache.GetUserById(AuthorId);

    private UserCacheItem? _author;

    public virtual QuestionChangeType Type { get; set; }

    public virtual DateTime DateCreated { get; set; }

    public virtual QuestionVisibility Visibility { get; set; }
    public virtual QuestionChangeData QuestionChangeData { get; set; }

    public virtual QuestionEditData GetQuestionChangeData()
    {
        switch (DataVersion)
        {
            case 1:
                return QuestionEditData_V1.CreateFromJson(Data);

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for question change id {Id}");
        }
    }

    public static QuestionChangeCacheItem ToQuestionChangeCacheItem(QuestionChange questionChange, QuestionEditData currentQuestionData, QuestionEditData? previousQuestionData)
    {
        var data = GetQuestionData(currentQuestionData, previousQuestionData);

        return new QuestionChangeCacheItem
        {
            Id = questionChange.Id,
            QuestionId = questionChange.Question.Id,
            Data = questionChange.Data,
            ShowInSidebar = questionChange.ShowInSidebar,
            DataVersion = questionChange.DataVersion,
            AuthorId = questionChange.AuthorId,
            Type = questionChange.Type,
            DateCreated = questionChange.DateCreated,
            Visibility = currentQuestionData.Visibility,
            QuestionChangeData = data
        };
    }

    private static QuestionChangeData GetQuestionData(QuestionEditData currentData, QuestionEditData? previousData)
    {
        return new QuestionChangeData(CommentIdsChange: GetCommentIds(currentData, previousData));
    }

    public static CommentIdsChange GetCommentIds(QuestionEditData currentData, QuestionEditData? previousData)
    {
        if (previousData == null || previousData.CommentIds == null || previousData.CommentIds?.Length == 0)
            return new CommentIdsChange(new List<int>(), currentData.CommentIds?.ToList());

        if (previousData.CommentIds?.Length > 0)
        {
            var newComments = currentData.CommentIds?.Except(previousData.CommentIds).ToList();
            if (newComments.Count > 0)
                return new CommentIdsChange(previousData.CommentIds.ToList(), currentData.CommentIds?.ToList());
        }

        return new CommentIdsChange(new List<int>(), new List<int>());
    }

    public virtual QuestionCacheItem ToHistoricQuestionCacheItem()
    {
        return GetQuestionChangeData().ToQuestionCacheItem(_questionCacheItem.Id);
    }
}
public record struct CommentIdsChange(List<int> OldCommentIds, List<int> NewCommentIds);

public record struct QuestionChangeData(CommentIdsChange CommentIdsChange);