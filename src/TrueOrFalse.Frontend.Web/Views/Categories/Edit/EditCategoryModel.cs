using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class EditCategoryModel : ModelBase
{
    public string Title;

    public List<Category> Categories; 

    
    public EditCategoryModel()
    {
        ShowLeftMenu_Nav();
    }

}
