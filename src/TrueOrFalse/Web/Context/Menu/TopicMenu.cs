using System;
using System.Collections.Generic;

[Serializable]
public class TopicMenu
{
    public bool IsActive = true;
    public List<Category> ActiveCategories;
    public List<Category> UserCategoryPath;
}