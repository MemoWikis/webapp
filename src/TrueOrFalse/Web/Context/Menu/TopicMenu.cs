using System;
using System.Collections.Generic;

[Serializable]
public class TopicMenu
{
    public bool IsActive = true;
    public IList<Category> PageCategories = new List<Category>();
    public IList<Category> CategoryPath = new List<Category>();
}