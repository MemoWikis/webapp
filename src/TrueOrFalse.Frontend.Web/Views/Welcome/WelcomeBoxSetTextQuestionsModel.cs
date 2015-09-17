using System.Collections.Generic;

public class WelcomeBoxSetTextQuestionsModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public string SetDescription;
    public int QuestionCount;
    public IList<Question> Questions;

    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxSetTextQuestionsModel(int setId, int[] questionIds, string setDescription = null) 
    {
        Set = R<SetRepo>().GetById(setId) ?? new Set();

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(Set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);


        SetName = Set.Name;
        SetDescription = setDescription ?? Set.Text;
        QuestionCount = Set.QuestionsInSet.Count;
        Questions = R<QuestionRepo>().GetByIds(questionIds) ?? new List<Question>(); //not checked if questionIds are part of set!
        if (Questions.Count < 1) Questions.Add(new Question());

    }

    public static WelcomeBoxSetTextQuestionsModel GetWelcomeBoxSetTextQuestionsModel(int setId, int[] questionIds,
        string setDescription = null)
    {
        return new WelcomeBoxSetTextQuestionsModel(setId, questionIds, setDescription);
    }
}
