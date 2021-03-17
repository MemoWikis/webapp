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

    public CategoryHistoryItem(CategoryCacheItem category, HistoryItemType type = HistoryItemType.Any, bool isCategoryNull = false)
    {
        Id = isCategoryNull ? -1 : category.Id;
        Name = isCategoryNull ? "" :  category.Name;
        Type = type;
    }

    public CategoryHistoryItem(Category category, HistoryItemType type = HistoryItemType.Any, bool isCategoryNull = false)
    {
        Id = isCategoryNull ? -1 : category.Id;
        Name = isCategoryNull ? "" : category.Name;
        Type = type;
    }
}

