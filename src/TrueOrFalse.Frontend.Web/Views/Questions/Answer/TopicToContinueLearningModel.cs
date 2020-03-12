using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TopicToContinueLearningModel
{
    public bool IsLearningSession { get; }
    public bool IsTestSession { get; }
    public ContentRecommendationResult ContentRecommendationResult { get; }
    public IList<Category> Categories; 
    public TopicToContinueLearningModel(AnswerQuestionModel answerQuestionModel, IList<Category> categoryList)
    {
        IsLearningSession = answerQuestionModel.IsLearningSession;
        IsTestSession = answerQuestionModel.IsTestSession;
        ContentRecommendationResult = answerQuestionModel.ContentRecommendationResult;
        Categories = categoryList;
    }

}
