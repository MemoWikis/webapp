using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class EditCategoryModel_Category_Exists : IRegisterAsInstancePerLifetime
{
    public Category ExistingCategory { get; private set; }

    public bool Yes(EditCategoryModel model)
    {
        return Yes(model.Name);
    }

    public bool Yes(string categoryName)
    {
        ExistingCategory = ServiceLocator.Resolve<CategoryRepository>().GetByName(categoryName);
        return ExistingCategory != null;        
    }
}