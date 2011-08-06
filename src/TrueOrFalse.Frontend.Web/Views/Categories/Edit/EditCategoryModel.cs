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

    public bool IsEditing { get; set; }

    public EditCategoryModel()
    {
        ShowLeftMenu_Nav();
    }

    public EditCategoryModel(Category category)
    {
        Name = category.Name;
        Classifications = (from classification in category.Classifications
                           select new ClassificationRowModel(classification)).ToList();
    }

    public Category ConvertToCategory()
    {
        return new Category(Name)
                   {
                       Classifications = (from model in Classifications select model.ConvertToClassification()).ToList()
                   };
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
        foreach (var classificationRowModel in Classifications)
        {
            Classification classification = null;
            if (classificationRowModel.Id > 0)
            {
                classification = category.Classifications.SingleOrDefault(c => c.Id == classificationRowModel.Id);
            }

            if (classification == null)
            {
                category.Classifications.Add(classificationRowModel.ConvertToClassification());
            }
            else
            {
                classificationRowModel.UpdateClassification(classification);
            }

        }
    }
}
