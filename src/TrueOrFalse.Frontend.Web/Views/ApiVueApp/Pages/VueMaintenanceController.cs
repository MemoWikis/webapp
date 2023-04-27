using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

namespace VueApp;

public class VueMaintenanceController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public JsonResult Get()
    {
        if (SessionUser.IsInstallationAdmin)
        {
            AntiForgery.GetTokens(null, out string cookieToken, out string formToken);
            HttpCookie antiForgeryCookie = new HttpCookie("__RequestVerificationToken");
            antiForgeryCookie.Value = cookieToken;
            antiForgeryCookie.HttpOnly = true;

            Response.Cookies.Add(antiForgeryCookie);

            return Json(new
            {
                isAdmin = true,
                antiForgeryFormToken = formToken
            },JsonRequestBehavior.AllowGet);
        }

        return Json(new { isAdmin = false }, JsonRequestBehavior.AllowGet);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult RecalculateAllKnowledgeItems()
    {
        ProbabilityUpdate_ValuationAll.Run();
        ProbabilityUpdate_Question.Run();
        ProbabilityUpdate_Category.Run();
        ProbabilityUpdate_User.Run();

        return Json(new
            {
                success = true,
                result = "Antwortwahrscheinlichkeiten wurden neu berechnet."
            });
    }


    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult CalcAggregatedValuesQuestions()
    {
        Resolve<UpdateQuestionAnswerCounts>().Run();


        return Json(new
        {
            success = true,
            result = "Aggregierte Werte wurden aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult UpdateFieldQuestionCountForTopics()
    {
        Resolve<UpdateQuestionCountForCategory>().All();

        return Json(new
        {
            success = true,
            result = "Feld: AnzahlFragen für Themen wurde aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult UpdateUserReputationAndRankings()
    {
        Resolve<ReputationUpdate>().RunForAll();

        return Json(new
        {
            success = true,
            result = "Reputation and Rankings wurden aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult UpdateUserWishCount()
    {
        Resolve<UpdateWishcount>().Run();

        return Json(new
        {
            success = true,
            result = "Wunschwissen-Antwortwahrscheinlichkeit wurde aktualisiert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReIndexAllQuestions()
    {
        Resolve<ReIndexAllQuestions>().Run();

        return Json(new
        {
            success = true,
            result = "Fragen wurden neu indiziert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReIndexAllTopics()
    {
        Resolve<SolrReIndexAllCategories>().Run();

        return Json(new
        {
            success = true,
            result = "Themen wurden neu indiziert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReIndexAllUsers()
    {
        Resolve<ReIndexAllUsers>().Run();

        return Json(new
        {
            success = true,
            result = "Nutzer wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllQuestions()
    {
        await Resolve<MeiliSearchReIndexAllQuestions>().Go();

        return Json(new
        {
            success = true,
            result = "Fragen wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllTopics()
    {
        await Resolve<MeiliSearchReIndexCategories>().Go();

        return Json(new
        {
            success = true,
            result = "Themen wurden neu indiziert.."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllUsers()
    {
        await Resolve<MeiliSearchReIndexAllUsers>().Run();

        return Json(new
        {
            success = true,
            result = "Nutzer wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult CheckForDuplicateInteractionNumbers()
    {
        var duplicates = Sl.R<AnswerRepo>().GetAll()
            .Where(a => a.QuestionViewGuid != Guid.Empty)
            .GroupBy(a => new { a.QuestionViewGuid, a.InteractionNumber })
            .Where(g => g.Skip(1).Any())
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any() ? "Es gibt Dubletten." : "Es gibt keine Dubletten.";

        return Json(new
        {
            success = true,
            result = message
        });
    }

    //Tools
}