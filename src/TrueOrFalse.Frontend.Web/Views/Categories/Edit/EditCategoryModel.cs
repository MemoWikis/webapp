using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class EditCategoryModel : ModelBase
{
    [DisplayName("Name")]
    public string Name { get; set; }

    public List<ClassificationRowModel> Classifications { get; set; } 

    public EditCategoryModel()
    {
        ShowLeftMenu_Nav();
    }

}
