using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Frontend.Web.Models;

public class CategoriesModel : ModelBase
{

    public List<ClassificationRowModel> Classifications; 

    public CategoriesModel()
    {
        ShowLeftMenu_Nav();
    }
}
