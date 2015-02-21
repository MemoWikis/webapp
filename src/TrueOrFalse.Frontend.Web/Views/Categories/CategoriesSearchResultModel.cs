using System.Collections.Generic;

public class CategoriesSearchResultModel : BaseModel
{
    public IEnumerable<CategoryRowModel> Rows { get; set; }
    public PagerModel Pager { get; set; }

    public CategoriesSearchResultModel(CategoriesModel categoriesModel)
    {
        Rows = categoriesModel.Rows;
        Pager = categoriesModel.Pager;
    }
}
