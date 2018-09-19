using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CategoryHistoryDetailModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public CategoryChange CategoryChange;
    public string AuthorName;
    public string AuthorImageUrl;

    public CategoryHistoryDetailModel(CategoryChange categoryChange)
    {
        CategoryChange = categoryChange;
        CategoryId = categoryChange.Category.Id;
        CategoryName = categoryChange.Category.Name;
        AuthorName = categoryChange.Author.Name;
        AuthorImageUrl = new UserImageSettings(categoryChange.Author.Id).GetUrl_85px_square(categoryChange.Author).Url;
    }
}

