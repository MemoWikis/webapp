using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryRowModel : BaseModel
{
    public Category Category;
    public int QuestionCount;
    public int CategoryId;
    public string CategoryName;
    public object DescriptionShort;

    public Func<UrlHelper, string> DetailLink;
    public bool UserCanEdit;

    public ImageFrontendData ImageFrontendData;

    public string DateCreated;
    public string DateCreatedLong;

    public int CorrectnesProbability;
    public int AnswersTotal;

    public CategoryRowModel(Category category, ReferenceCountPair referenceCountPair)
    {
        Category = category;
        CategoryId = category.Id;
        CategoryName = category.Name;
        DescriptionShort = "";

        QuestionCount = 
            category.CountQuestions + 
            (referenceCountPair == null ? 0 : referenceCountPair.ReferenceCount);

        UserCanEdit = _sessionUser.IsInstallationAdmin;

        DetailLink = urlHelper => Links.CategoryDetail(category.Name, category.Id);

        DateCreated = category.DateCreated.ToString("dd.MM.yyyy");
        DateCreatedLong = category.DateCreated.ToString("U");//Change to "g" format?

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;
    }
}