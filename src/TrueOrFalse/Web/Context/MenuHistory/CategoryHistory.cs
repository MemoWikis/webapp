using System;
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

    public CategoryHistoryItem(Category category, HistoryItemType type = HistoryItemType.Any, Data data = null, bool isCategoryNull = false)
    {
        Id = isCategoryNull ? data.Id : category.Id;
        Name = isCategoryNull ? data.Name :  category.Name;
        Type = type;
    }
}

namespace TrueOrFalse
{
    public class Data
    {
        public int Id;
        public string Name;
       
    }
}
