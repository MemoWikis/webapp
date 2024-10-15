using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Seedworks.Lib.Persistence;

public class QuestionChange : Entity, WithDateCreated
{
    public virtual Question Question { get; set; }
    public virtual string Data { get; set; }

    public virtual bool ShowInSidebar { get; set; } = true;

    public virtual int DataVersion { get; set; }

    public virtual int AuthorId { get; set; }

    public virtual UserCacheItem Author(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        => _author ??= EntityCache.GetUserById(AuthorId);

    private UserCacheItem? _author;

    public virtual QuestionChangeType Type { get; set; }

    public virtual DateTime DateCreated { get; set; }


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

    public virtual Question ToHistoricQuestion()
    {
        return GetQuestionChangeData().ToQuestion(Question);
    }
}

public enum QuestionChangeType
{
    Create = 0,
    Update = 1,
    Delete = 2,
    AddComment = 3,
}