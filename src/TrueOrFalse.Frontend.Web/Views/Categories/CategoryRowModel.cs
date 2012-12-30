using TrueOrFalse;

public class CategoryRowModel
{
    public CategoryRowModel(Category category)
    {
        CategoryId = category.Id;
        CategoryName = category.Name;
        QuestionCount = category.QuestionCount;
    }

    public int QuestionCount { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}