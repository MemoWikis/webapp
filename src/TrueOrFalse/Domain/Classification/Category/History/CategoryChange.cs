using System;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

public class CategoryChange : Entity, WithDateCreated
{
    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }
    public virtual User Author { get; set; }
    public virtual DateTime DateCreated { get; set; }

    public virtual CategoryEditData_V1 GetCategeoryEditData() => JsonConvert.DeserializeObject<CategoryEditData_V1>(Data);
}