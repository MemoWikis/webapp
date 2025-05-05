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
    //public virtual List<QuestionChangeCacheItem> GroupedQuestionChangeCacheItems { get; set; } = new List<QuestionChangeCacheItem>();
    //public virtual bool IsGroup => GroupedQuestionChangeCacheItems.Count > 1;
    //public virtual bool IsPartOfGroup { get; set; } = false;

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

    //public static bool CanBeGrouped(QuestionChangeCacheItem previousCacheItem, QuestionChangeCacheItem currentCacheItem)
    //{
    //    var timeSpan = 10;

    //    var allowedGroupingTypes = new List<QuestionChangeType>
    //    {
    //        QuestionChangeType.Update
    //    };

    //    return allowedGroupingTypes.Contains(previousCacheItem.Type)
    //           && Math.Abs((previousCacheItem.DateCreated - currentCacheItem.DateCreated).TotalMinutes) <= timeSpan
    //           && previousCacheItem.AuthorId == currentCacheItem.AuthorId
    //           && previousCacheItem.Visibility == currentCacheItem.Visibility
    //           && previousCacheItem.Type == currentCacheItem.Type;
    //}

    //public static QuestionChangeCacheItem ToGroupQuestionChangeCacheItem(List<QuestionChangeCacheItem> groupedCacheItems)
    //{
    //    var oldestPageChangeItem = groupedCacheItems.First();
    //    var newestPageChangeItem = groupedCacheItems.Last();

    //    var currentQuestionData = newestPageChangeItem.GetQuestionChangeData();
    //    var data = GetQuestionData(currentQuestionData, oldestPageChangeItem.GetQuestionChangeData());

    //    return new QuestionChangeCacheItem
    //    {
    //        Id = newestPageChangeItem.Id,
    //        QuestionId = newestPageChangeItem.Question.Id,
    //        Data = newestPageChangeItem.Data,
    //        ShowInSidebar = newestPageChangeItem.ShowInSidebar,
    //        DataVersion = newestPageChangeItem.DataVersion,
    //        AuthorId = newestPageChangeItem.AuthorId,
    //        Type = newestPageChangeItem.Type,
    //        DateCreated = newestPageChangeItem.DateCreated,
    //        Visibility = currentQuestionData.Visibility,
    //        QuestionChangeData = data
    //    };
    //}
}
public record struct CommentIdsChange(List<int> OldCommentIds, List<int> NewCommentIds);

public record struct QuestionChangeData(CommentIdsChange CommentIdsChange);