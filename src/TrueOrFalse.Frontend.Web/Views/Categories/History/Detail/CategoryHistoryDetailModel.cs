using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CategoryHistoryDetailModel : BaseModel
{
    public int CategoryId;

    public CategoryHistoryDetailModel(CategoryChange categoryChange)
    {
        CategoryId = categoryChange.Category.Id;
    }
}
