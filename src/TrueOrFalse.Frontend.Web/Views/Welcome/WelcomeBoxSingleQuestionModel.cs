public class WelcomeBoxSingleQuestionModel : BaseModel
{
    public int QuestionId;
    public string QuestionText;

    public int ContextCategoryId;
    public string ContextCategoryName = "";
    public int QuestionCount;


    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxSingleQuestionModel(int questionId, int contextCategoryId = 0) 
    {
        var question = R<QuestionRepo>().GetById(questionId) ?? new Question();

        ImageFrontendData = GetQuestionImageFrontendData.Run(question);

        QuestionText = question.Text;
        QuestionId = question.Id;
        ContextCategoryId = contextCategoryId;

        if (ContextCategoryId != 0)
        {
            var contextCategory = R<CategoryRepository>().GetById(contextCategoryId);
            if (contextCategory != null)
            {
                ContextCategoryName = contextCategory.Name;
                QuestionCount = contextCategory.CountQuestionsAggregated;
            } 
        }

    }

    public static WelcomeBoxSingleQuestionModel GetWelcomeBoxQuestionVModel(int questionId, int contextCatId = 0)
    {
        return new WelcomeBoxSingleQuestionModel(questionId, contextCatId);
    }
}
