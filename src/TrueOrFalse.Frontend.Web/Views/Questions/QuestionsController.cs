﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Criterion;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
using TrueOrFalse;
using TrueOrFalse.Search;

namespace TrueOrFalse
{

    public class QuestionsController : BaseController
    {
        private readonly QuestionRepository _questionRepository;
        private readonly QuestionsControllerSearch _questionsControllerSearch;

        public QuestionsController(QuestionRepository questionRepository,
                                   QuestionsControllerSearch questionsControllerSearch)
        {
            _questionRepository = questionRepository;
            _questionsControllerSearch = questionsControllerSearch;
        }

        [SetMenu(MenuEntry.Questions)]
        public ActionResult Questions(int? page, QuestionsModel model, string orderBy)
        {
            SetSearchSpecVars(_sessionUiData.SearchSpecQuestionAll, page, model, orderBy);

            return View("Questions",
                new QuestionsModel(
                    _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionAll), 
                    _sessionUiData.SearchSpecQuestionAll, 
                    isTabAllActive: true));
        }

        public ActionResult QuestionsSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            SetSearchTerm(_sessionUiData.SearchSpecQuestionAll, model, searchTerm);
            return Questions(page, model, orderBy);
        }

        [SetMenu(MenuEntry.Questions)]
        public ActionResult QuestionsMine(int? page, QuestionsModel model, string orderBy)
        {
            if (!_sessionUser.IsLoggedIn){
                return View("Questions",
                    new QuestionsModel(new List<Question>(), new QuestionSearchSpec(), isTabMineActive: true));
            }

            return View("Questions", GetQuestionsModel(page, model, orderBy, _sessionUiData.SearchSpecQuestionMine, isTabMineActive: true));
        }

        public ActionResult QuestionsMineSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            SetSearchTerm(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);
            return QuestionsMine(page, model, orderBy);
        }



        public JsonResult QuestionsMineSearchApi(string searchTerm)
        {
            var model = new QuestionsModel();
            SetSearchTerm(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);

            return SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionMine, isTabMineActive: true);
        }

        public JsonResult SearchApi(
            string searchTerm, 
            QuestionSearchSpec searchSpec,
            bool isTabAllActive = false,
            bool isTabWishActive = false,
            bool isTabMineActive = false)
        {
            var model = new QuestionsModel();
            SetSearchTerm(searchSpec, model, searchTerm);

            return new JsonResult
            {
                Data = new
                {
                    Html = ViewRenderer.RenderPartialView(
                        "QuestionsSearchResult",
                        new QuestionsSearchResultModel(
                            GetQuestionsModel(
                                searchSpec.CurrentPage, model,
                                "",
                                searchSpec,
                                isTabAllActive,
                                isTabWishActive,
                                isTabMineActive
                                )),
                        this.ControllerContext)
                },
            };
        }

        private QuestionsModel GetQuestionsModel(
            int? page, 
            QuestionsModel model, 
            string orderBy, 
            QuestionSearchSpec searchSpec,
            bool isTabAllActive = false,
            bool isTabWishActive = false,
            bool isTabMineActive = false)
        {
            SetSearchSpecVars(searchSpec, page, model, orderBy);
            searchSpec.Filter.CreatorId = _sessionUser.User.Id;

            var questionsModel = new QuestionsModel(
                _questionsControllerSearch.Run(searchSpec),
                searchSpec,
                isTabAllActive,
                isTabWishActive,
                isTabMineActive);

            return questionsModel;
        }

        [SetMenu(MenuEntry.Questions)]
        public ActionResult QuestionsWish(int? page, QuestionsModel model, string orderBy)
        {
            if (!_sessionUser.IsLoggedIn){
                return View("Questions", 
                    new QuestionsModel(new List<Question>(), new QuestionSearchSpec(), isTabWishActive: true));
            }

            SetSearchSpecVars(_sessionUiData.SearchSpecQuestionWish, page, model, orderBy);
            _sessionUiData.SearchSpecQuestionWish.Filter.ValuatorId = _sessionUser.User.Id;

            return View("Questions",
                new QuestionsModel(
                    _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionWish),
                    _sessionUiData.SearchSpecQuestionWish,
                    isTabWishActive: true));
        }

        public ActionResult QuestionsWishSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            SetSearchTerm(_sessionUiData.SearchSpecQuestionWish, model, searchTerm);
            return QuestionsWish(page, model, orderBy);
        }

        public JsonResult QuestionsWishSearchApi(string searchTerm)
        {
            var model = new QuestionsModel();
            SetSearchTerm(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);

            return SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionMine, isTabWishActive: true);
        }

        [HttpPost]
        public JsonResult DeleteDetails(int questionId)
        {
            var question = _questionRepository.GetById(questionId);

            return new JsonResult{
                Data = new{
                    questionTitle = question.Text.WordWrap(50),
                    totalAnswers = question.TotalAnswers()
                }
            };
        }

        [HttpPost]
        public EmptyResult Delete(int questionId)
        {
            Sl.Resolve<QuestionDeleter>().Run(questionId);
            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult GetQuestionSets(string filter)
        {
            var searchSpec = new SetSearchSpec{PageSize = 12};
            var searchResult = Resolve<SearchSets>().Run(
                filter, searchSpec, _sessionUser.User.Id, startsWithSearch:true);
            var questionSets = Resolve<SetRepository>().GetByIds(searchResult.SetIds.ToArray());

            return new JsonResult{
                Data = new{
                   Sets = questionSets.Select(s => new{Id = s.Id, Name = s.Name, QuestionCount = s.QuestionsInSet.Count}),
                   Total = searchResult.Count
                }
            };
        }

        [HttpPost]
        public JsonResult AddToQuestionSet()
        {
            var parts = Request.Form[0].Split(':');
            var questionIds = parts[0].Split(',').Select(id => Convert.ToInt32(id)).ToArray();
            var questionSetId = Convert.ToInt32(parts[1]);
        
            var result = Resolve<AddToSet>().Run(questionIds, questionSetId);
        
            return new JsonResult{ Data = new{
                QuestionsAddedCount = result.AmountAddedQuestions,
                QuestionAlreadyInSet = result.AmountOfQuestionsAlreadyInSet
            }};
        }

        public void SetSearchTerm(QuestionSearchSpec searchSpec, QuestionsModel model, string searchTerm)
        {
            if (searchSpec.Filter.SearchTerm != searchTerm)
                searchSpec.CurrentPage = 1;

            if(!String.IsNullOrEmpty(searchTerm))
                searchTerm = searchTerm
                    .Replace("Kat__", "Kat:\"")
                    .Replace("Ersteller__", "Ersteller:\"")
                    .Replace("__", "\"")
                    .Replace("___", ":")
                    .Replace("_and_", "&");

            searchSpec.Filter.SearchTerm = model.SearchTerm = searchTerm;
        }

        public void SetSearchSpecVars(QuestionSearchSpec searchSpec, int? page, QuestionsModel model,
            string orderBy, string defaultOrder = "byRelevance")
        {
            searchSpec.PageSize = 20;

            if (page.HasValue)
                searchSpec.CurrentPage = page.Value;

            SetOrderBy(searchSpec, orderBy, defaultOrder);        
        }

        public void SetOrderBy(QuestionSearchSpec searchSpec, string orderByCommand, string defaultOrder)
        {
            if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
                orderByCommand = defaultOrder;

            if (orderByCommand == "byRelevance") searchSpec.OrderBy.OrderByPersonalRelevance.Desc();
            else if (orderByCommand == "byQuality") searchSpec.OrderBy.OrderByQuality.Desc();
            else if (orderByCommand == "byDateCreated") searchSpec.OrderBy.OrderByCreationDate.Desc();
            else if (orderByCommand == "byViews") searchSpec.OrderBy.OrderByViews.Desc();
        }
    }

}