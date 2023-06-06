using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicController : BaseController
{

    private readonly CategoryRepository _categoryRepository = Sl.CategoryRepo;

    public EditTopicController(SessionUser sessionUser): base(sessionUser)
    {
        
    }

}