using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

public class ClassificationRowModel
{
    [DisplayName("Name")]
    public string Name { get; set; }

    [DisplayName("Type")]
    public string Type { get; set; }

    public IEnumerable<SelectListItem> TypeData
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Offene Liste", Value = ClassificationType.OpenList.ToString()},
                            new SelectListItem {Text = "Geschlossene Liste", Value = ClassificationType.ClosedList.ToString()},
                        };
        }
    }

    public Classification ConvertToClassification()
    {
        return new Classification(Name)
                   {
                       Type = (ClassificationType) Enum.Parse(typeof(ClassificationType), Type)
                   };
    }
}