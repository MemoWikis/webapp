using System;
using Seedworks.Lib.Persistence;

public class CategoryChange : Entity, WithDateCreated
{
    public int DataVersion;
    public CategoryEditData_V1 Data;
    public User Author;
    public DateTime DateCreated { get; set; }
}