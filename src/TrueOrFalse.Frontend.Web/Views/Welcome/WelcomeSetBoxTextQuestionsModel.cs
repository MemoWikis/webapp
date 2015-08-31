using System.Collections.Generic;
using System.Linq;

public class WelcomeSetBoxTextQuestionsModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public int QuestionCount;
    public IList<Question> Questions;

    public ImageFrontendData ImageFrontendData;

    public WelcomeSetBoxTextQuestionsModel(int setId, int[] questionIds) 
    {

        var set = R<SetRepo>().GetById(setId);
        if (set == null) //if set doesn't exist, take new empty set to avoid broken starting page
        {
            set = new Set();
        }
        Set = set;

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);


        SetName = set.Name;
        QuestionCount = set.QuestionsInSet.Count;
        Questions = R<QuestionRepository>().GetByIds(questionIds); //not checked if questionIds are part of set!

        //set.Questions().Where((currentQuestion) => {currentQuestion.Id ==  1})
        //var q = set.Questions().Where((currentQuestion) => questionIds.Contains(currentQuestion.Id)).ToList();


        /*if (ContextCategoryId != 0)
        {
            var contextCategory = R<CategoryRepository>().GetById(contextCategoryId);
            if (contextCategory != null)
            {
                //ContextCategory = contextCategory;
                ContextCategoryName = contextCategory.Name;
            } 
        }*/

    }
}
