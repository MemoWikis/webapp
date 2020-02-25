using System.Collections.Generic;
using System.Linq;

public class CategoryCardModel
{
    public IList<Category> QuestionIsInCategorys;

    public CategoryCardModel(IList<Category> categorys, IList<Category> parentCategorys, int primaryCategoryId)
    {
        QuestionIsInCategorys = categorys;

        var categoryParents = parentCategorys;
        var categoryChildrens = Sl.CategoryRepo.GetChildren(primaryCategoryId);
        QuestionIsInCategorys = QuestionIsInCategorys.Concat(categoryParents).Concat(categoryChildrens).Distinct().ToList();
    }
}
