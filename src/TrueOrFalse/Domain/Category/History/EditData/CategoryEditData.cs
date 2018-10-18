using System.Collections.Generic;

public abstract class CategoryEditData
{
    public string Name;
    public string Description;
    public string TopicMarkdown;
    public string WikipediaURL;
    public bool DisableLearningFunctions;

    public abstract string ToJson();
    //public abstract Category ToCategory(int categoryId);

    public Category ToCategory(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);

        Sl.Session.Evict(category);

        category.Name = this.Name;
        category.Description = this.Description;
        category.TopicMarkdown = this.TopicMarkdown;
        category.WikipediaURL = this.WikipediaURL;
        category.DisableLearningFunctions = this.DisableLearningFunctions;

        //we decided not to load historic CategoryRelations
        //because it seems to complicated
        //category.CategoryRelations = this.CategoryRelations.ToList();

        return category;
    }
}