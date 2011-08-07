using System.Collections.Generic;
using TrueOrFalse.Core;

public class ExportCategoriesModel
{

    public IEnumerable<Category> Categories { get; set; }

    public ExportCategoriesModel(IEnumerable<Category> categories)
    {
        Categories = categories;
    }
}