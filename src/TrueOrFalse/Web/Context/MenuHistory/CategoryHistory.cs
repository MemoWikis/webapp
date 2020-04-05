using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TrueOrFalse;


[Serializable]
public class CategoryHistory : HistoryBase<CategoryHistoryItem>
{
}

[Serializable]
public class CategoryHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public HistoryItemType Type { get; set; }

    public CategoryHistoryItem(Category category, HistoryItemType type = HistoryItemType.Any, Data data = null)
    {
        Id = data.IsCategoryNull ? data.Id : category.Id;
        Name = data.IsCategoryNull ? data.Name :  category.Name;
        Type = type;
    }
}

namespace TrueOrFalse
{
    public class Data
    {
        public int Id;
        public string Name;
        public bool IsCategoryNull;

    }
}
