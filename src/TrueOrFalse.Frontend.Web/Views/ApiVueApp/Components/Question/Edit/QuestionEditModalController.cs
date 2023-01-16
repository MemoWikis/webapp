using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
public class QuestionEditModalController : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public QuestionEditModalController(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }


    [HttpGet]
    public JsonResult GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var categoryController = new CategoryController();
        var solution = question.SolutionType == SolutionType.FlashCard ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml() : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(PermissionCheck.CanView).Distinct();

        return Json(new
        {
            SolutionType = (int)question.SolutionType,
            Solution = solution,
            SolutionMetadataJson = question.SolutionMetadataJson,
            Text = question.TextHtml,
            TextExtended = question.TextExtendedHtml,
            TopicIds = topicsVisibleToCurrentUser.Select(t => t.Id).ToList(),
            DescriptionHtml = question.DescriptionHtml,
            Topics = topicsVisibleToCurrentUser.Select(t => FillMiniTopicItem(t)),
            LicenseId = question.LicenseId,
            Visibility = question.Visibility,
        }, JsonRequestBehavior.AllowGet);
    }
    public SearchCategoryItem FillMiniTopicItem(CategoryCacheItem category)
    {
        var miniTopicItem = new SearchCategoryItem
        {
            Id = category.Id,
            Name = category.Name,
            Url = Links.CategoryDetail(category.Name, category.Id),
            QuestionCount = category.GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(category.Id).GetUrl_128px(asSquare: true).Url,
            IconHtml = SearchApiController.GetIconHtml(category),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)category.Visibility
        };

        return miniTopicItem;
    }
}