using System.Collections.Generic;

public abstract class CategoryEditData
{
    public string Name;
    public string Description;
    public string TopicMardkown;
    public string WikipediaURL;
    public bool DisableLearningFunctions;

    public abstract string ToJson();

    public abstract Category ToCategory(int categoryId);
}