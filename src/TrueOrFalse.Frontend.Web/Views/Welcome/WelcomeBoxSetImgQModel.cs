﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class WelcomeBoxSetImgQModel : BaseModel
{
    public int SetId;
    public Set Set;
    public string SetName;
    public string SetText;
    public int QuestionCount;
    public IList<Question> Questions;
    public IList<Tuple<int, ImageFrontendData>> QuestionImageFrontendDatas;
    public Func<UrlHelper, string> SetDetailLink;

    public bool IsInWishknowledge;

    public WelcomeBoxSetImgQModel(int setId, int[] questionIds, string setText = null) 
    {
        Set = R<SetRepo>().GetById(setId) ?? new Set();
        SetId = Set.Id;
        SetName = Set.Name;
        SetText = setText ?? Set.Text;
        QuestionCount = Set.QuestionsInSet.Count;
        Questions = R<QuestionRepo>().GetByIds(questionIds); //not checked if questionIds are part of set!
        QuestionImageFrontendDatas = Questions.Select(x => new Tuple<int, ImageFrontendData>(
            x.Id, GetQuestionImageFrontendData.Run(Questions.ById(x.Id)))
        ).ToList();
        IsInWishknowledge = R<SetValuationRepo>().GetBy(setId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;
        SetDetailLink = urlHelper => Links.SetDetail(urlHelper, Set);

    }

    public static WelcomeBoxSetImgQModel GetWelcomeBoxSetImgQModel(int setId, int[] questionIds,
        string setText = null)
    {
        return new WelcomeBoxSetImgQModel(setId, questionIds, setText);
    }
}
