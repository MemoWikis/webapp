using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

public class SubCategoryRowModel
{
    public SubCategoryRowModel(SubCategory subCategory)
    {
        Name = subCategory.Name;
        Type = subCategory.Type.ToString();
        Id = subCategory.Id;
    }

    public SubCategoryRowModel()
    {
    }

    [DisplayName("Name")]
    public string Name { get; set; }

    [DisplayName("Type")]
    public string Type { get; set; }

    [DisplayName("ItemCount")]
    public int ItemCount { get; set; }

    public int Id { get; set; }

    public bool IsNew
    {
        get { return Id == 0; }
    }

    public IEnumerable<SelectListItem> TypeData
    {
        get
        {
            var items = new List<SelectListItem>
                                      {
                                          new SelectListItem {Text = "Offene Liste", Value = SubCategoryType.OpenList.ToString()},
                                          new SelectListItem {Text = "Geschlossene Liste", Value = SubCategoryType.ClosedList.ToString()},
                                      };
            var selectedItem = items.SingleOrDefault(item => item.Value == Type);
            if (selectedItem != null) selectedItem.Selected = true;
            return items;
        }
    }

    public SubCategory ConvertToSubCategory()
    {
        return new SubCategory(Name)
                   {
                       Type = (SubCategoryType) Enum.Parse(typeof(SubCategoryType), Type)
                   };
    }

    public void UpdateSubCategory(SubCategory subCategory)
    {
        subCategory.Name = Name;
        subCategory.Type = (SubCategoryType) Enum.Parse(typeof (SubCategoryType), Type);
    }
}