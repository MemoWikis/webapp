using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Code;

namespace TrueOrFalse.View.Web.Views.Administrative
{
    [AccessOnlyLocal]
    public class PersonaController : Controller
    {
        private readonly SessionUser _sessionUser;
        private readonly SampleData _sampleData;
        private readonly UserRepository _userRepository;

        public PersonaController(SessionUser sessionUser,
                                 SampleData sampleData, 
                                 UserRepository userRepository)
        {
            _sessionUser = sessionUser;
            _sampleData = sampleData;
            _userRepository = userRepository;
        }

        public ActionResult Robert()
        {
            SessionFactory.BuildSchema();
            _sampleData.CreateUsers();
            var robertM = _userRepository.GetByUserName("RobertM");
            _sessionUser.Login(robertM);

            return RedirectToAction(Links.Summary, Links.SummaryController);
        }
    }
}