public class WelcomeQuestionBoxVModel : BaseModel
{
    public int Id;
    public string Text;
    public Question Question;  //used only to display categories with PartialView Category.ascx

    public int ContextCategoryId;
    //public Category ContextCategory;
    public string ContextCategoryName = "";


    public ImageFrontendData ImageFrontendData;

    public WelcomeQuestionBoxVModel(Question question, int contextCategoryId) 
    {

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(question.Id, ImageType.Question);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        Text = question.Text;
        Question = question;
        ContextCategoryId = contextCategoryId;

        if (ContextCategoryId != 0)
        {
            var contextCategory = R<CategoryRepository>().GetById(contextCategoryId);
            if (contextCategory != null)
            {
                //ContextCategory = contextCategory;
                ContextCategoryName = contextCategory.Name;
            } 
        }

    }
}
