using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class SetModel : BaseModel
{
    public int Id;
    public string Name;

    public Set Set;
    
    public IList<QuestionInSet> QuestionsInSet;
    public IList<TotalPerUser> TotalsPerUser;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public string ImageUrl;

    public bool IsOwner;

    public Func<UrlHelper, string> DetailLink;

    public int AnswersAllCount;
    public int AnswersAllPercentageTrue;
    public int AnswersAllPercentageFalse;

    public int AnswerMeCount;
    public int AnswerMePercentageTrue;
    public int AnswerMePercentageFalse;

    public bool IsInWishknowledge;

    public string TotalPins;

    public SetModel(Set set)
    {
        Id = set.Id;
        Name = set.Name;

        Set = set;
        QuestionsInSet = set.QuestionsInSet;

        IsOwner = _sessionUser.IsOwner(set.Creator.Id);

        Creator = set.Creator;
        CreatorName = set.Creator.Name;
        CreationDate = set.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = TimeElapsedAsText.Run(set.DateCreated);

        ImageUrl = QuestionSetImageSettings.Create(set.Id).GetUrl_350px_square().Url;

        var questions = set.QuestionsInSet.Select(x => x.Question).ToList();
        TotalsPerUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.User.Id, questions);

        AnswersAllCount = questions.Sum(q => q.TotalAnswers());
        AnswersAllPercentageTrue = questions.Sum(q => q.TotalTrueAnswersPercentage());
        AnswersAllPercentageFalse = questions.Sum(q => q.TotalFalseAnswerPercentage());

        AnswerMeCount = TotalsPerUser.Sum(q => q.Total());
        AnswerMePercentageTrue = TotalsPerUser.Sum(q => q.TotalTrue);
        AnswerMePercentageFalse = TotalsPerUser.Sum(q => q.TotalFalse);

        var setValuations = Resolve<SetValuationRepository>().GetBy(Id);
        var setValuation = setValuations.FirstOrDefault(sv => sv.UserId == _sessionUser.UserId);
        if (setValuation != null){
            IsInWishknowledge = setValuation.IsInWishknowledge();
        }

        TotalPins = set.TotalRelevancePersonalEntries.ToString();
    }
    
}
