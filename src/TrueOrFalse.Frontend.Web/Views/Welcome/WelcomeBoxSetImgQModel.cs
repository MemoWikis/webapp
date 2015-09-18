using System.Collections.Generic;
using System.Linq;

public class WelcomeBoxSetImgQModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public string SetText;
    public int QuestionCount;
    public IList<Question> Questions;
    public IList<ImageFrontendData> QuestionImageFrontendDatas;

    public WelcomeBoxSetImgQModel(int setId, int[] questionIds, string setText = null) 
    {
        Set = R<SetRepo>().GetById(setId) ?? new Set();
        SetName = Set.Name;
        SetText = setText ?? Set.Text;
        QuestionCount = Set.QuestionsInSet.Count;
        Questions = R<QuestionRepo>().GetByIds(questionIds); //not checked if questionIds are part of set!
        QuestionImageFrontendDatas = Resolve<ImageMetaDataRepository>().GetBy(questionIds.ToList(), ImageType.Question).Select(e => new ImageFrontendData(e)).ToList();
    }

    public static WelcomeBoxSetImgQModel GetWelcomeBoxSetImgQModel(int setId, int[] questionIds,
        string setText = null)
    {
        return new WelcomeBoxSetImgQModel(setId, questionIds, setText);
    }
}
