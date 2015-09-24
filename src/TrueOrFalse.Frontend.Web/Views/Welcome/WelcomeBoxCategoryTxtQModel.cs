using System.Collections.Generic;

public class WelcomeBoxCategoryTxtQModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public string CategoryDescription;
    public int QuestionCount;
    public IList<Question> Questions;

    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxCategoryTxtQModel(int categoryId, int[] questionIds, string categoryDescription = null) 
    {
        var category = R<CategoryRepository>().GetById(categoryId) ?? new Category();

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);


        CategoryId = category.Id;
        CategoryName = category.Name;
        CategoryDescription = categoryDescription ?? category.Description;
        QuestionCount = category.CountQuestions;
        Questions = R<QuestionRepo>().GetByIds(questionIds) ?? new List<Question>(); //not checked if questionIds are part of category!
        if (Questions.Count < 1) Questions.Add(new Question());

    }

    public static WelcomeBoxCategoryTxtQModel GetWelcomeBoxCategoryTxtQModel(int categoryId, int[] questionIds, string categoryDescription = null)
    {
        return new WelcomeBoxCategoryTxtQModel(categoryId, questionIds, categoryDescription);
    }
}
