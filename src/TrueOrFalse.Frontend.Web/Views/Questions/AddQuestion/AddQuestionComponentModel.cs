//using TrueOrFalse.Frontend.Web.Code;

public class AddQuestionComponentModel : BaseModel
{
    public int CategoryId;
    public string CreateQuestionUrl;

    public AddQuestionComponentModel(int categoryId)
    {
        CategoryId = categoryId;
        //CreateQuestionUrl = Links.CreateQuestion(categoryId: CategoryId);
    }
}