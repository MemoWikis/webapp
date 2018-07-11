using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.View.Web.Views.Knowledge.Partials;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

public class KnowledgeController : BaseController
{

    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        return View(new KnowledgeModel());
    }

    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult EmailConfirmation(string emailKey)
    {
        return View("Knowledge", new KnowledgeModel(emailKey: emailKey));
    }


    public int GetNumberOfWishknowledgeQuestions()
    {
        if (_sessionUser.User != null)
        {
            return Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, true);
        }
        else
            return -1;
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession()
    {
        var user = _sessionUser.User;
        if (user.WishCountQuestions == 0)
            throw new Exception("Cannot start LearningSession from Wishknowledge with no questions.");

        var valuations = Sl.QuestionValuationRepo
            .GetByUserFromCache(user.Id)
            .QuestionIds().ToList();
        var wishQuestions = Resolve<QuestionRepo>().GetByIds(valuations);

        // if User has uncompleted WishSession that is less than 3 hours old, then continue this one. Else: Start new one
        var openWishSession = Sl.R<LearningSessionRepo>().GetLastWishSessionIfUncompleted(user);

        if (openWishSession != null)
        {
            if (DateTime.Now - openWishSession.DateModified < new TimeSpan(0, 5, 0))
                return Redirect(Links.LearningSession(openWishSession));
            openWishSession.CompleteSession();
        }

        var learningSession = new LearningSession
        {
            IsWishSession = true,
            Steps = GetLearningSessionSteps.Run(wishQuestions),
            User = user
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    public String GetKnowledgeContent(string content)
    {
        

        switch (content)
        {
            case "dashboard": return ViewRenderer.RenderPartialView("~/Views/Knowledge/Partials/_Dashboard.ascx", new KnowledgeModel(), ControllerContext);
            case "topics": return ViewRenderer.RenderPartialView("~/Views/Knowledge/Partials/KnowledgeTopics.ascx", new KnowledgeTopicsModel(), ControllerContext);
            case "questions": return ViewRenderer.RenderPartialView("~/Views/Knowledge/Partials/_Dashboard.ascx", new KnowledgeModel(), ControllerContext);
            default: throw new ArgumentException("Argument false or null");
        }
    }

    public int GetDatesCount(string userId)
    {
        var Dates = R<DateRepo>().GetBy(Int32.Parse(userId), true);
        return Dates.Count - 1; // if last date is deleted counter is still 1  
        //after deleting, however, there is no longer an appointment
    }


    // get CategoryDates 
    // begin
    public JsonResult getCatsAndSetsWish()
    {
        var filteredCategoryWishKnowledges = filteredCategoryWishKnowledge();
        return new JsonResult {Data = filteredCategoryWishKnowledges};
    }

    public List<CategoryWishKnowledge> filteredCategoryWishKnowledge()
    {
        List<CategoryWishKnowledge> filteredCategoryWishKnowledges = new List<CategoryWishKnowledge>();
      
        var categoryValuationIds = UserValuationCache.GetCategoryValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(v => v.CategoryId)
            .ToList();
        
        var categoriesWishPool = R<CategoryRepository>().GetByIds(categoryValuationIds);
        var categoriesWish = OrderCategoriesByQuestionCountAndLevel.Run(categoriesWishPool);

        foreach (var categoryWish in categoriesWish)
        { 
            var categoryWishKnowledge = new CategoryWishKnowledge();
            categoryWishKnowledge.CategoryDescription = categoryWish.Description;
            categoryWishKnowledge.CategoryTitel = categoryWish.Name;
            categoryWishKnowledge.ImageFrontendData = GetCategoryImage(categoryWish.Id);
            categoryWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(categoryWish);

            filteredCategoryWishKnowledges.Add(categoryWishKnowledge);
        }

        return filteredCategoryWishKnowledges;
    }

    public ImageFrontendData GetCategoryImage(int CategoryId)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    public string KnowledgeWishPartial(Category category)
    {
        var KnowledgeBarPartial =ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category),ControllerContext);
        
        return KnowledgeBarPartial;
        
    }
    public class CategoryWishKnowledge
    {
        public string CategoryDescription;
        public string CategoryTitel;
        public ImageFrontendData ImageFrontendData;
        public string KnowlegdeWishPartial;

    }
  // end
}
