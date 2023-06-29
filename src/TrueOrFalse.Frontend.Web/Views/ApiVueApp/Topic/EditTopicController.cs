using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicController : BaseController
{

    private readonly CategoryRepository _categoryRepository;

    public EditTopicController(SessionUser sessionUser, CategoryRepository categoryRepository): base(sessionUser)
    {
        _categoryRepository = categoryRepository;
    }

}