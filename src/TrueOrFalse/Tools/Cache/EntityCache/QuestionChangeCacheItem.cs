using Newtonsoft.Json.Linq;
using Seedworks.Lib.Persistence;

[Serializable]
public class QuestionChangeCacheItem : IPersistable
{
    public virtual int Id { get; set; }

    private QuestionCacheItem? _questionCacheItem;
    public virtual QuestionCacheItem Category => _questionCacheItem ??= EntityCache.GetQuestion(QuestionId);
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

    public static QuestionChangeCacheItem ToQuestionChangeCacheItem(QuestionChange questionChange)
    {
        var visibility = QuestionVisibility.Owner;

        if (!string.IsNullOrEmpty(questionChange.Data))
        {
            var jObject = JObject.Parse(questionChange.Data);
            if (jObject["Visibility"] != null)
            {
                var visibilityValue = jObject["Visibility"].Value<int>();
                visibility = (QuestionVisibility)visibilityValue;
            }
        }

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
            Visibility = visibility
        };
    }

    public virtual QuestionCacheItem ToHistoricQuestionCacheItem()
    {
        return GetQuestionChangeData().ToQuestionCacheItem(_questionCacheItem.Id);
    }
}