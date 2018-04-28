using System.Collections.Generic;
using System.Linq;

public class KnowledgeWheelController : BaseController
{
    public string GetForSet(int setId)
    {
        var set = Sl.SetRepo.GetById(setId);
        var knowledgeSummary = KnowledgeSummaryLoader.Run(UserId, set);
        return RenderPartialView(knowledgeSummary);
    }

    public string GetForCategory(int categoryId)
    {
        var knowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(categoryId, UserId);
        return RenderPartialView(knowledgeSummary);
    }

   

    public string GetForAddedCategoryTemp(int categoryId)
    {
        var questions = Sl.QuestionRepo.GetForCategoryAggregated(categoryId, UserId);
        var probResults = new List<ProbabilityCalcResult>();
        foreach (var question in questions)
        {
            var result = Sl.R<ProbabilityCalc_Simple1>().Run(question, User_());
            if (result != null)
            {
                probResults.Add(result);
            }
        }

        var knowledgeSummary = new KnowledgeSummary(
            0,
            probResults.Count(r => r.KnowledgeStatus == KnowledgeStatus.NotLearned),
            probResults.Count(r => r.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            probResults.Count(r => r.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            probResults.Count(r => r.KnowledgeStatus == KnowledgeStatus.Solid)
        );
        return RenderPartialView(knowledgeSummary);
    }

    private string RenderPartialView(KnowledgeSummary knowledgeSummary) => 
        ViewRenderer.RenderPartialView(
            "/Views/Knowledge/Wheel/KnowledgeWheel.ascx",
            knowledgeSummary,
            ControllerContext
        );
}