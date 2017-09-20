using System;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

public class CategoryChange : Entity, WithDateCreated
{
    public int DataVersion;
    public string Data;
    public User Author;
    public DateTime DateCreated { get; set; }

    public CategoryEditData_V1 GetCategeoryEditData() => JsonConvert.DeserializeObject<CategoryEditData_V1>(Data);
}