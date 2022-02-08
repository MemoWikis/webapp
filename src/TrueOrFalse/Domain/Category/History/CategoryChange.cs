using Seedworks.Lib.Persistence;
using System;

public class CategoryChange : Entity, WithDateCreated
{
    public virtual Category Category { get; set; }
    public virtual int ParentCategoryId { get; set; }
    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    public virtual bool ShowInSidebar { get; set; } = true;

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

    public virtual Category ToHistoricCategory(bool haveVersionData = true)
    {
        return haveVersionData ? GetCategoryChangeData().ToCategory(Category.Id) : new Category();
    }
}

public enum CategoryChangeType
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
}