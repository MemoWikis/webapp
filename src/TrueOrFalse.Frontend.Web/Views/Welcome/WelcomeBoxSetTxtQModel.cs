using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class WelcomeBoxSetTxtQModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public string SetDescription;
    public int QuestionCount;
    public IList<Question> Questions;

    public ImageFrontendData ImageFrontendData;

    public Func<UrlHelper, string> SetDetailLink;

    public bool IsInWishknowledge;

    public WelcomeBoxSetTxtQModel(int setId, int[] questionIds, string setDescription = null) 
    {
        Set = R<SetRepo>().GetById(setId) ?? new Set();
        SetId = Set.Id;

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(Set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        SetName = Set.Name;
        SetDescription = setDescription ?? Set.Text;
        QuestionCount = Set.QuestionsInSet.Count;
        Questions = R<QuestionRepo>().GetByIds(questionIds) ?? new List<Question>(); //not checked if questionIds are part of set!
        if (Questions.Count < 1)
            Questions.Add(new Question());
        IsInWishknowledge = R<SetValuationRepo>().GetBy(setId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;
        SetDetailLink = urlHelper => Links.SetDetail(urlHelper, Set);
    }

    public static WelcomeBoxSetTxtQModel GetWelcomeBoxSetTxtQModel(int setId, int[] questionIds,
        string setDescription = null)
    {
        return new WelcomeBoxSetTxtQModel(setId, questionIds, setDescription);
    }
}
