using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class TopicOfWeekModel : BaseModel
{
    public Category Category;
    public int CategoryId;
    public string TopicOfWeekTitle;
    public string TopicDescriptionHTML;
    public ImageFrontendData ImageFrontendData;

    public int QuestionCount;
    public bool IsInWishknowledge;

    public int QuizOfWeekSetId;
    //public IList<int> AdditionalSetsIds;
    public IList<int> AdditionalCategoriesIds;


    public TopicOfWeekModel(DateTime date)
    {
        var topicOfWeek = TopicOfWeekRepo.GetTopicOfWeek(date);
        CategoryId = topicOfWeek.CategoryId;
        Category = R<CategoryRepository>().GetById(CategoryId) ?? new Category();
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        TopicOfWeekTitle = String.IsNullOrEmpty(topicOfWeek.TopicOfWeekTitle) ? Category.Name : topicOfWeek.TopicOfWeekTitle;
        if (String.IsNullOrEmpty(topicOfWeek.TopicDescriptionHTML))
        {
            TopicDescriptionHTML = string.IsNullOrEmpty(Category.Description?.Trim())
                ? null
                : MarkdownMarkdig.ToHtml(Category.Description);
            TopicDescriptionHTML = "<p>" + TopicDescriptionHTML + "</p>";
        } else {
            TopicDescriptionHTML = topicOfWeek.TopicDescriptionHTML;
        }
        //TopicDescriptionHTML = String.IsNullOrEmpty(topicOfWeek.TopicDescriptionHTML) ? "<p>" + Category.Description + "</p>" : topicOfWeek.TopicDescriptionHTML;

        QuizOfWeekSetId = topicOfWeek.QuizOfWeekSetId;
        AdditionalCategoriesIds = topicOfWeek.AdditionalCategoriesIds;

    }
}
