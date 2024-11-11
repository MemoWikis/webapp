using Seedworks.Lib.Persistence;

public class PageChange : Entity, WithDateCreated
{
    public virtual Page? Page { get; set; }
    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    public virtual bool ShowInSidebar { get; set; } = true;
    private UserCacheItem? _author;

    public virtual UserCacheItem? Author() => _author ??= EntityCache.GetUserById(AuthorId);

    public virtual int AuthorId { get; set; }

    public virtual PageChangeType Type { get; set; }

    public virtual DateTime DateCreated { get; set; }

    public virtual PageEditData GetCategoryChangeData()
    {
        switch (DataVersion)
        {
            case 1:
                return PageEditDataV1.CreateFromJson(Data);

            case 2:
                return PageEditData_V2.CreateFromJson(Data);

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for category change id {Id}");
        }
    }

    public virtual Page ToHistoricCategory(bool haveVersionData = true)
    {
        return haveVersionData ? GetCategoryChangeData().ToPage(Page.Id) : new Page();
    }
}

public enum PageChangeType
{
    Create = 0,
    Update = 1,
    Delete = 2,
    Published = 3,
    Privatized = 4,
    Renamed = 5,
    Text = 6,
    Relations = 7,
    Image = 8,
    Restore = 9,
    Moved = 10,
    ChildTopicDeleted = 11,
    QuestionDeleted = 12,
}