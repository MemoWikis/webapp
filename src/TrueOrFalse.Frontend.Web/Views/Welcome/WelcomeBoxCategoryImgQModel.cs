using System.Collections.Generic;
using System.Linq;

public class WelcomeBoxCategoryImgQModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public string CategoryDescription;
    public int QuestionCount;
    public IList<Question> Questions;
    public IList<ImageFrontendData> QuestionImageFrontendDatas;

    public WelcomeBoxCategoryImgQModel(int categoryId, int[] questionIds, string categoryDescription = null) 
    {
        var category = R<CategoryRepository>().GetById(categoryId) ?? new Category("Kategorie nicht gefunden");
        CategoryId = category.Id;
        CategoryName = category.Name;
        CategoryDescription = categoryDescription ?? category.Description;
        QuestionCount = category.CountQuestions;

        Questions = R<QuestionRepo>().GetByIds(questionIds); //not checked if questionIds are part of category!
        QuestionImageFrontendDatas = Resolve<ImageMetaDataRepository>().GetBy(questionIds.ToList(), ImageType.Question).Select(e => new ImageFrontendData(e)).ToList();
    }

    public static WelcomeBoxCategoryImgQModel GetWelcomeBoxCategoryImgQModel(int categoryId, int[] questionIds, string categoryDescription = null)
    {
        return new WelcomeBoxCategoryImgQModel(categoryId, questionIds, categoryDescription);
    }
}
