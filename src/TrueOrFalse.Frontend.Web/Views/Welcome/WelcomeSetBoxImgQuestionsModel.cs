using System.Collections.Generic;
using System.Linq;

public class WelcomeSetBoxImgQuestionsModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public int QuestionCount;
    public IList<Question> Questions;
    public IList<ImageFrontendData> QuestionImageFrontendDatas;

    public WelcomeSetBoxImgQuestionsModel(int setId, int[] questionIds) 
    {
        Set = R<SetRepo>().GetById(setId) ?? new Set();
        SetName = Set.Name;
        QuestionCount = Set.QuestionsInSet.Count;
        Questions = R<QuestionRepository>().GetByIds(questionIds); //not checked if questionIds are part of set!
        QuestionImageFrontendDatas = Resolve<ImageMetaDataRepository>().GetBy(questionIds.ToList(), ImageType.Question).Select(e => new ImageFrontendData(e)).ToList();
    }
}
