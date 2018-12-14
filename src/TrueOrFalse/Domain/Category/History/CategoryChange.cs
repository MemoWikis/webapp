using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryChange : Entity, WithDateCreated
{
    public virtual Category Category { get; set; }
    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    public virtual User Author { get; set; }

    public virtual CategoryChangeType Type { get; set; } 

    public virtual DateTime DateCreated { get; set; }

    public virtual void SetData(Category category, bool imageWasUpdated)
    {
        switch (DataVersion)
        {
            case 1:
                Data = new CategoryEditData_V1(category).ToJson();
                break;

            case 2:
                Data = new CategoryEditData_V2(category, imageWasUpdated).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for category change id {Id}");
        }
    }

    public virtual CategoryEditData GetCategoryChangeData()
    {
        switch (DataVersion)
        {
            case 1:
                return CategoryEditData_V1.CreateFromJson(Data);
                
            case 2:
                return CategoryEditData_V2.CreateFromJson(Data);
                
            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for category change id {Id}");
        }
    }

    public virtual Category ToHistoricCategory()
    {
        return GetCategoryChangeData().ToCategory(Category.Id);
    }

    public virtual CategoryChange GetNextRevision()
    {
        var categoryId = Category.Id;
        var currentRevisionDate = DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated > '{currentRevisionDate}' 
            ORDER BY cc.DateCreated 
            LIMIT 1

            ";
        var nextRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).UniqueResult<CategoryChange>();
        return nextRevision;
    }

    public virtual CategoryChange GetPreviousRevision()
    {
        var categoryId = Category.Id;
        var currentRevisionDate = DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated < '{currentRevisionDate}' 
            ORDER BY cc.DateCreated DESC 
            LIMIT 1

            ";
        var previousRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).UniqueResult<CategoryChange>();
        return previousRevision;
    }
}

public enum CategoryChangeType
{
    Create = 0,
    Update = 1,
    Delete = 2
}