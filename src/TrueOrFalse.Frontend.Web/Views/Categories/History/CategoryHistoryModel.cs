using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CategoryHistoryModel : BaseModel
{
    public string CategoryName;

    public CategoryHistoryModel(Category category)
    {
        CategoryName = category.Name;
    }
}