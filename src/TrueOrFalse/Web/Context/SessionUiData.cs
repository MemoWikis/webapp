using System.Collections.Generic;
using Seedworks.Web.State;

public class SessionUiData : IRegisterAsInstancePerLifetime
{
    private readonly SessionData _sessionData;

    public SessionUiData(SessionData sessionData)
    {
        _sessionData = sessionData;
    }
    public QuestionSearchSpec SearchSpecQuestionSearchBox => _sessionData.Get("searchSpecQuestionSearchBox", new QuestionSearchSpec{Key = "searchbox"});

    public List<QuestionSearchSpec> SearchSpecQuestions => _sessionData.Get("searchSpecQuestions", new List<QuestionSearchSpec>());
    public TmpImageStore TmpImagesStore => _sessionData.Get("tmpImageStore", new TmpImageStore());
}