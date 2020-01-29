using System.Collections.Generic;
using System.Linq;

public class CategoryCardModel
{
    public IList<Category> QuestionIsInCategorys;
    public bool NeedParentsOrChildrens;

    public CategoryCardModel(IList<Category> categorys, IList<Category> parentCategorys, int primaryCategoryId, bool needParentsOrChildrens = false)
    {
        QuestionIsInCategorys = categorys;
        NeedParentsOrChildrens = needParentsOrChildrens; 

        if (needParentsOrChildrens)
        {
            QuestionIsInCategorys.Clear();
            var categoryParents = parentCategorys;
            var categoryChildrens = Sl.CategoryRepo.GetChildren(primaryCategoryId);
            QuestionIsInCategorys = categoryChildrens.Concat(categoryParents).ToList();
        }
    }
}
