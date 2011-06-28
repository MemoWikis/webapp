using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Frontend.Web.Models;
using Message = TrueOrFalse.Core.Web.Message;

namespace TrueOrFalse.View.Web.Views.Question
{
    [HandleError]
    public class CreateQuestionController : Controller
    {
        private readonly QuestionRepository _questionRepository;
        private const string _viewLocation = "~/Views/Question/CreateQuestion/CreateQuestion.aspx";

        public CreateQuestionController(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        
        public ActionResult Create()
        {
            var model = new CreateQuestionModel();
            model.Answer = "Antwort eingeben";
            model.Question = "Frage eingeben";

            return View(_viewLocation, model);
        }

        [HttpPost]
        public ActionResult Create(CreateQuestionModel model)
        {
            ViewData["question"] = model.Question;

            _questionRepository.Create(model.ConvertToQuestion());

            model.Message = new SuccessMessage("Die Nachricht wurde gespeichert");

            return View(_viewLocation, model);
        }

    }
}