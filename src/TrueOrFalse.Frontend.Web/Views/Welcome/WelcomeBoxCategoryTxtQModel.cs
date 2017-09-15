using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class WelcomeBoxCategoryTxtQModel : BaseModel
{
    public Category Category;
    public int CategoryId;
    public string CategoryName;
    public string CategoryDescription;
    public int QuestionCount;
    public IList<Question> Questions;
    public Func<UrlHelper, string> CategoryDetailLink;

    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxCategoryTxtQModel(int categoryId, int[] questionIds, string categoryDescription = null) 
    {
        var category = R<CategoryRepository>().GetById(categoryId) ?? new Category();
        Category = category;

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);


        CategoryId = category.Id;
        CategoryName = category.Name;
        CategoryDescription = categoryDescription ?? category.Description;
        CategoryDetailLink = urlHelper => Links.CategoryDetail(category.Name, category.Id);
        QuestionCount = category.CountQuestionsAggregated;
        Questions = R<QuestionRepo>().GetByIds(questionIds) ?? new List<Question>(); //not checked if questionIds are part of category!
        if (Questions.Count < 1) Questions.Add(new Question());

    }

    public static WelcomeBoxCategoryTxtQModel GetWelcomeBoxCategoryTxtQModel(int categoryId, int[] questionIds, string categoryDescription = null)
    {
        return new WelcomeBoxCategoryTxtQModel(categoryId, questionIds, categoryDescription);
    }
}
