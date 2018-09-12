using System;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

public class CategoryChange : Entity, WithDateCreated
{
    public virtual Category Category { get; set; }
    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    public virtual User Author { get; set; }

    public virtual CategoryChangeType Type { get; set; } 

    public virtual DateTime DateCreated { get; set; }

    public void SetData(Category category) => Data = new CategoryEditData_V1(category).ToJson();

    public CategoryEditData_V1 GetCategoryChangeData() => CategoryEditData_V1.CreateFromJson(Data);
}

public enum CategoryChangeType
{
    Create = 0,
    Update = 1,
    Delete = 2
}