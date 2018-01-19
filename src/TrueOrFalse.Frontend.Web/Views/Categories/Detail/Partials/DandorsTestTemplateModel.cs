using System;
using System.Collections.Generic;
using System.Linq;

public class DandorsTestTemplateModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public bool HasUsedOrderListWithLoadList;

    public DandorsTestTemplateModel()
    {
     
    }

  
}