using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;

public class AnswerQuestionModel : BaseModel
{
    public Func<UrlHelper, string> NextUrl;
    public Guid QuestionViewGuid;
    public bool AnswerHelp; 

    public int QuestionId;
    public QuestionCacheItem Question;
    public UserTinyModel Creator;
    public string CreatorId { get; private set; }
    public string SolutionType;

    public string QuestionText { get; private set; }
    public string QuestionTitle { get; private set; }
    public QuestionVisibility Visibility { get; private set; }
    public bool HasNextPage;

    public IList<CategoryCacheItem> Categories;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public IList<CommentModel> Comments;

    public bool IsLearningSession => LearningSession != null;
    public LearningSession  LearningSession;

    public int TestSessionProgessAfterAnswering;

    public bool DisableCommentLink;
    public bool DisableAddKnowledgeButton;
    public bool IsInWidget;

    public ContentRecommendationResult ContentRecommendationResult;

    public AnswerQuestionModel(QuestionCacheItem question, bool isQuestionDetails)
    {
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(UserId, question.Id);
        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, UserId));

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };
    }
}
