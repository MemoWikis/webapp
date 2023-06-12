using System.Collections.Generic;
using Seedworks.Web.State;

public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
{
    public QuestionSearchSpec SearchSpecQuestionSearchBox => SessionDataLegacy.Get("searchSpecQuestionSearchBox", new QuestionSearchSpec{Key = "searchbox"});

    public List<QuestionSearchSpec> SearchSpecQuestions => SessionDataLegacy.Get("searchSpecQuestions", new List<QuestionSearchSpec>());
    public TmpImageStore TmpImagesStore => SessionDataLegacy.Get("tmpImageStore", new TmpImageStore());
}