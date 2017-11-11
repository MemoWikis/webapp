using System;
using System.Collections.Generic;
using System.Linq;

public class MediaListModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> MediaList;

    public MediaListModel(Category category, string title, string text = null)
    {
        Category = category;
        Title = title ?? "Medien";
        Text = text;
        MediaList = Sl.CategoryRepo.GetChildren(category.Id).Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media).ToList(); //use GetDescendents instead of GetChildren?
    }

    public int GetTotalQuestionCount(Category category)
    {
        return ReferenceCount.GetInclCategorizedQuestions(category);
    }

    public int GetTotalSetCount(Category category)
    {
        return category.GetAggregatedSetsFromMemoryCache().Count;
    }
}