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

    public IList<ClassificationRowModel> Classifications { get; set; } 

    public EditCategoryModel()
    {
        ShowLeftMenu_Nav();
    }

    public Category ConvertToCategory()
    {
        return new Category(Name)
                   {
                       Classifications = (from model in Classifications select model.ConvertToClassification()).ToList()
                   };
    }
}
