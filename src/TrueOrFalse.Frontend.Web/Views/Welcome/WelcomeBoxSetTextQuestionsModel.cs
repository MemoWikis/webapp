using System.Collections.Generic;
using System.Linq;

public class WelcomeBoxSetTextQuestionsModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public int QuestionCount;
    public IList<Question> Questions;

    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxSetTextQuestionsModel(int setId, int[] questionIds) 
    {
        var set = R<SetRepo>().GetById(setId) ?? new Set();
        Set = set;

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);


        SetName = set.Name;
        QuestionCount = set.QuestionsInSet.Count;
        Questions = R<QuestionRepository>().GetByIds(questionIds); //not checked if questionIds are part of set!

    }
}
