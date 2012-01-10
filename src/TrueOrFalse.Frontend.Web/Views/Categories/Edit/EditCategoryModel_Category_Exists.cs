using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;

public class EditCategoryModel_Category_Exists : IRegisterAsInstancePerLifetime
{
    public Category ExistingCategory { get; private set; }

    public bool Yes(EditCategoryModel model)
    {
        ExistingCategory = ServiceLocator.Resolve<CategoryRepository>().GetByName(model.Name);
        return ExistingCategory != null;
    }
}