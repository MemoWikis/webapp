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

    //public virtual QuestionChange GetNextRevision()
    //{
    //    var categoryId = Category.Id;
    //    var currentRevisionDate = DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
    //    var query = $@"
            
    //        SELECT * FROM QuestionChange cc
    //        WHERE cc.Category_id = {categoryId} and cc.DateCreated > '{currentRevisionDate}' 
    //        ORDER BY cc.DateCreated 
    //        LIMIT 1

    //        ";
    //    var nextRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(QuestionChange)).UniqueResult<QuestionChange>();
    //    return nextRevision;
    //}

    //public virtual QuestionChange GetPreviousRevision()
    //{
    //    var categoryId = Category.Id;
    //    var currentRevisionDate = DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
    //    var query = $@"
            
    //        SELECT * FROM QuestionChange cc
    //        WHERE cc.Category_id = {categoryId} and cc.DateCreated < '{currentRevisionDate}' 
    //        ORDER BY cc.DateCreated DESC 
    //        LIMIT 1

    //        ";
    //    var previousRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(QuestionChange)).UniqueResult<QuestionChange>();
    //    return previousRevision;
    //}
}

public enum QuestionChangeType
{
    Create = 0,
    Update = 1,
    Delete = 2
}