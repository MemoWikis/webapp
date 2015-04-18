using System.Collections.Generic;

public class ExportModel 
{

    public IEnumerable<Question> Questions { get; set; }
    public IEnumerable<Category> Categories { get; set; }

    public ExportModel(IEnumerable<Question> questions, IEnumerable<Category> categories)
    {
        Questions = questions;
        Categories = categories;
    }
}