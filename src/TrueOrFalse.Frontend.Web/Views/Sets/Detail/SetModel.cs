using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;

public class SetModel : BaseModel
{
    public int Id;
    public string Name;

    public Set Set;

    public IList<SetQuestionRow> QuestionsInSet;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public ImageFrontendData ImageFrontendData;

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

    public SetActiveMemory ActiveMemory;

    public SetModel(Set set)
    {
        Id = set.Id;
        Name = set.Name;

        Set = set;
        
        IsOwner = _sessionUser.IsLoggedInUser(set.Creator.Id);

        Creator = set.Creator;
        CreatorName = set.Creator.Name;
        CreationDate = set.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = TimeElapsedAsText.Run(set.DateCreated);

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        var questionValutionsForCurrentUser = Resolve<QuestionValuationRepo>()
            .GetActiveInWishknowledge(set.QuestionsInSet.Select(x => x.Question.Id).ToList(), _sessionUser.UserId);

        var questions = set.QuestionsInSet.Select(x => x.Question).ToList();
        var totalsPerUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, questions);
        QuestionsInSet = set.QuestionsInSet.Select(
            x => new SetQuestionRow(
                x.Question, 
                totalsPerUser.ByQuestionId(x.Question.Id),
                questionValutionsForCurrentUser.ByQuestionId(x.Question.Id)))
            .ToList();

        AnswersAllCount = questions.Sum(q => q.TotalAnswers());
        AnswersAllPercentageTrue = questions.Sum(q => q.TotalTrueAnswersPercentage());
        AnswersAllPercentageFalse = questions.Sum(q => q.TotalFalseAnswerPercentage());

        AnswerMeCount = totalsPerUser.Sum(q => q.Total());
        AnswerMePercentageTrue = totalsPerUser.Sum(q => q.TotalTrue);
        AnswerMePercentageFalse = totalsPerUser.Sum(q => q.TotalFalse);

        var setValuations = Resolve<SetValuationRepo>().GetBy(Id);
        var setValuation = setValuations.FirstOrDefault(sv => sv.UserId == _sessionUser.UserId);
        if (setValuation != null){
            IsInWishknowledge = setValuation.IsInWishknowledge();
        }

        TotalPins = set.TotalRelevancePersonalEntries.ToString();

        ActiveMemory = SetActiveMemoryLoader.Run(set, questionValutionsForCurrentUser);
    }
    
}
