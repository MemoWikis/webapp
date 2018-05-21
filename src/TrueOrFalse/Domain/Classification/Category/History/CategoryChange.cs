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

    public virtual CategoryEditData_V1 GetCategeoryEditData() => JsonConvert.DeserializeObject<CategoryEditData_V1>(Data);
}

public enum CategoryChangeType
{
    Update = 1,
    Delete = 2
}