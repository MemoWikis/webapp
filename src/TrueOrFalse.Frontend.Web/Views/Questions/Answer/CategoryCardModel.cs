using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class CategoryCardModel
{
    public IList<Category> Category;
    public IList<Category> QuestionInCategory;
    public bool NeedParentsOrChildrens; 

    public CategoryCardModel(IList<Category> questionInCategory, IList<Category> parentCategorys, int primaryCategoryId, bool needParentsOrChildrens = false)
    {
        
        QuestionInCategory = questionInCategory;
        NeedParentsOrChildrens = needParentsOrChildrens; 

        if (needParentsOrChildrens)
        {
            QuestionInCategory.Clear();
            var categoryParents = parentCategorys;
            var categoryChildrens = Sl.CategoryRepo.GetChildren(primaryCategoryId);
            QuestionInCategory = categoryChildrens.Concat(categoryParents).ToList();
        }
    }
}
