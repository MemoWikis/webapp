public class WelcomeBoxSingleQuestionModel : BaseModel
{
    public int QuestionId;
    public string QuestionText;

    public int ContextCategoryId;
    public string ContextCategoryName = "";
    public int QCount;


    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxSingleQuestionModel(int questionId, int contextCategoryId = 0) 
    {
        var question = R<QuestionRepo>().GetById(questionId) ?? new Question();

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(question.Id, ImageType.Question);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        QuestionText = question.Text;
        QuestionId = question.Id;
        ContextCategoryId = contextCategoryId;

        if (ContextCategoryId != 0)
        {
            var contextCategory = R<CategoryRepository>().GetById(contextCategoryId);
            if (contextCategory != null)
            {
                ContextCategoryName = contextCategory.Name;
                QCount = contextCategory.CountQuestions;
            } 
        }

    }

    public static WelcomeBoxSingleQuestionModel GetWelcomeBoxQuestionVModel(int questionId, int contextCatId = 0)
    {
        return new WelcomeBoxSingleQuestionModel(questionId, contextCatId);
    }
}
