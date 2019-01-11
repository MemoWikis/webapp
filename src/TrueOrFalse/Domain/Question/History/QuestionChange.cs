using System;
using Seedworks.Lib.Persistence;

public class QuestionChange : Entity, WithDateCreated
{
    public virtual Question Question { get; set; }
    public virtual string Data { get; set; }
    public virtual int DataVersion { get; set; }

    public virtual User Author { get; set; }

    public virtual QuestionChangeType Type { get; set; } 

    public virtual DateTime DateCreated { get; set; }

    public virtual void SetData(Question question)
    {
        switch (DataVersion)
        {
            case 1:
                Data = new QuestionEditData_V1(question).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for question change id {Id}");
        }
    }

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

    //public virtual Category ToHistoricCategory()
    //{
    //    return GetQuestionChangeData().ToCategory(Category.Id);
    //}
}

public enum QuestionChangeType
{
    Create = 0,
    Update = 1,
    Delete = 2
}