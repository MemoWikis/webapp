using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryRowModel : BaseModel
{
    public Category Category;
    public int QuestionCount;
    public int SetCount;
    public int CategoryId;
    public string CategoryName;
    public object DescriptionShort;
    public bool IsMediaCategory;
    public bool IsEducationCategory;
    public string CategoryTypeName;
    public bool HasMarkdownContent;

    public Func<UrlHelper, string> DetailLink;
    public bool UserCanEdit;

    public ImageFrontendData ImageFrontendData;

    public string DateCreated;
    public string DateCreatedLong;

    public int CorrectnesProbability;
    public int AnswersTotal;

    public bool IsInWishknowledge;

    public CategoryRowModel(Category category, CategoryValuation valution)
    {
        Category = category;
        CategoryId = category.Id;
        CategoryName = category.Name;
        DescriptionShort = "";
        var catTypeGroup = Category.Type.GetCategoryTypeGroup();
        IsMediaCategory = catTypeGroup == CategoryTypeGroup.Media;
        IsEducationCategory = catTypeGroup == CategoryTypeGroup.Education;
        CategoryTypeName = Category.Type.GetName();
        HasMarkdownContent = !string.IsNullOrEmpty(category.TopicMarkdown);

        IsInWishknowledge = valution.IsInWishKnowledge();

        QuestionCount = category.GetCountQuestions();
        SetCount = category.GetCountSets();

        UserCanEdit = _sessionUser.IsInstallationAdmin;

        DetailLink = urlHelper => Links.CategoryDetail(category.Name, category.Id);

        DateCreated = category.DateCreated.ToString("dd.MM.yyyy");
        DateCreatedLong = category.DateCreated.ToString("U");//Change to "g" format?

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;
    }
}