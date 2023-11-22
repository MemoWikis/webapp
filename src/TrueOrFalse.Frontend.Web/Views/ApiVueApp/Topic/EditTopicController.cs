namespace VueApp;

public class EditTopicController : BaseController
{

    private readonly CategoryRepository _categoryRepository;

    public EditTopicController(SessionUser sessionUser, CategoryRepository categoryRepository): base(sessionUser)
    {
        _categoryRepository = categoryRepository;
    }

}