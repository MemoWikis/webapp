using System;
using System.Collections.Generic;

[Serializable]
public class TopicMenu
{
    public bool IsActive = true;
    public IList<Category> ActiveCategories = new List<Category>();
    public IList<Category> UserCategoryPath = new List<Category>();
}