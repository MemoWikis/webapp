using System;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class KnowledgeReportMsgModel
{
    public DateTime LastSent;

    public string EmailTitle = "Dein wöchentlicher Wissensbericht";

    public string QuestionCountWish;
    public string SetCountWish;

    public string KnowledgeSolid;
    public string KnowledgeSolidPercentage;
    public string KnowledgeNeedsConsolidation;
    public string KnowledgeNeedsConsolidationPercentage;
    public string KnowledgeNeedsLearning;
    public string KnowledgeNeedsLearningPercentage;
    public string KnowledgeNotLearned;
    public string KnowledgeNotLearnedPercentage;

    public string KnowledgeLastLearnedDate;
    public string KnowledgeLastLearnedDateAsDistance;

    public string NewQuestions;
    public string NewSets;
    public string TotalAvailableQuestions;
    public string TotalAvailableSets;

    public string LinkToLearningSession;
    public string LinkToDates;
    public string LinkToTechInfo;

    public string UtmSourceFullString;
    public string UtmCampaignFullString = "";

    public KnowledgeReportMsgModel(User user, string utmSource = "knowledgeReportEmail")
    {
        LastSent = DateTime.Now.AddDays(-7); //todo: needs to be adapted if user can chose how often to receive update
        UtmSourceFullString = string.IsNullOrEmpty(utmSource) ? "" : "&utm_source=" + utmSource;

        QuestionCountWish = user.WishCountQuestions + " Frage" + StringUtils.PluralSuffix(user.WishCountQuestions, "n");
        SetCountWish = user.WishCountSets + " Frage" + StringUtils.PluralSuffix(user.WishCountSets, "sätze", "satz");

        var knowledgeSummary = KnowledgeSummaryLoader.Run(user.Id);
        KnowledgeSolid = knowledgeSummary.Solid.ToString();
        KnowledgeSolidPercentage = knowledgeSummary.SolidPercentage.ToString();
        KnowledgeNeedsConsolidation = knowledgeSummary.NeedsConsolidation.ToString();
        KnowledgeNeedsConsolidationPercentage = knowledgeSummary.NeedsConsolidationPercentage.ToString();
        KnowledgeNeedsLearning = knowledgeSummary.NeedsLearning.ToString();
        KnowledgeNeedsLearningPercentage = knowledgeSummary.NeedsLearningPercentage.ToString();
        KnowledgeNotLearned = knowledgeSummary.NotLearned.ToString();
        KnowledgeNotLearnedPercentage = knowledgeSummary.NotLearnedPercentage.ToString();

        var knowledgeLastLearnedDate = Sl.R<LearningSessionRepo>().GetDateOfLastWishSession(user);// ?? DateTime.Now.AddDays(1);
        if (knowledgeLastLearnedDate == null)
        {
            KnowledgeLastLearnedDate = "noch nie";
            KnowledgeLastLearnedDateAsDistance = "";
        }
        else
        {
            KnowledgeLastLearnedDate = knowledgeLastLearnedDate?.ToString("'am' dd.MM.yyyy 'um' HH: mm");
            var remainingLabel = new TimeSpanLabel(DateTime.Now - (knowledgeLastLearnedDate ?? DateTime.Now));
            KnowledgeLastLearnedDateAsDistance = " (" + remainingLabel.Full + ")";
        }

        NewQuestions = Sl.R<QuestionRepo>().HowManyNewQuestionsCreatedSince(LastSent).ToString();
        NewSets = "XX";

        TotalAvailableQuestions = "XX";
        TotalAvailableSets = "XX";

        /*Should use Links.XX to create links, but Tests fail if UrlHelper trys to access HttpContext.Current.Request.RequestContext 
        //LinkToDates = Links.Dates();
        //LinkToTechInfo = Links.AlgoInsightForecast();*/
        LinkToLearningSession = "https://memucho.de/Lernen/Wunschwissen";
        LinkToDates = "https://memucho.de/Termine";
        LinkToTechInfo = "https://memucho.de/AlgoInsight/Forecast";
    }
}