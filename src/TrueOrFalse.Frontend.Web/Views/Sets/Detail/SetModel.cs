using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;

public class SetModel : BaseModel
{
    public int Id;
    public string Name;
    public string Text;

    public Set Set;

    public IList<SetQuestionRowModel> QuestionsInSet;
    public int QuestionCount;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public ImageFrontendData ImageFrontendData;

    public bool IsOwner;
    public bool IsLoggedIn;

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

    public ContentRecommendationResult ContentRecommendationResult;


    public SetModel(Set set)
    {
        Id = set.Id;
        Name = set.Name;
        Text = set.Text;

        Set = set;

        ImageMetaDataCache.WarmupRequestCache(set);

        var foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        IsOwner = _sessionUser.IsLoggedInUser(set.Creator.Id);
        IsLoggedIn = _sessionUser.IsLoggedIn;

        Creator = set.Creator;
        CreatorName = set.Creator.Name;
        CreationDate = set.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(set.DateCreated);

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        var questionValutionsForCurrentUser = Resolve<QuestionValuationRepo>()
            .GetActiveInWishknowledge(set.QuestionsInSet.Select(x => x.Question.Id).ToList(), _sessionUser.UserId);

        var questions = set.QuestionsInSet.Select(x => x.Question).ToList();
        var totalsPerUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, questions);

        QuestionsInSet = set.QuestionsInSet.Select(
            x => new SetQuestionRowModel(
                x.Question, 
                x.Set,
                totalsPerUser.ByQuestionId(x.Question.Id),
                questionValutionsForCurrentUser.ByQuestionId(x.Question.Id)))
            .ToList();

        QuestionCount = QuestionsInSet.Count;

        foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        AnswersAllCount = questions.Sum(q => q.TotalAnswers());
        AnswersAllPercentageTrue = questions.Sum(q => q.TotalTrueAnswersPercentage());
        AnswersAllPercentageFalse = questions.Sum(q => q.TotalFalseAnswerPercentage());

        AnswerMeCount = totalsPerUser.Sum(q => q.Total());
        AnswerMePercentageTrue = totalsPerUser.Sum(q => q.TotalTrue);
        AnswerMePercentageFalse = totalsPerUser.Sum(q => q.TotalFalse);

        foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        var setValuations = Resolve<SetValuationRepo>().GetBy(Id);
        var setValuation = setValuations.FirstOrDefault(sv => sv.UserId == _sessionUser.UserId);
        if (setValuation != null){
            IsInWishknowledge = setValuation.IsInWishKnowledge();
        }

        foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        TotalPins = set.TotalRelevancePersonalEntries.ToString();

        ActiveMemory = SetActiveMemoryLoader.Run(set, questionValutionsForCurrentUser);

        ContentRecommendationResult = ContentRecommendation.GetForSet(Set, 6);
    }

}
