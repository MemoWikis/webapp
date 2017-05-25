using System;
using System.Collections.Generic;
using System.Linq;

public class SetVideoModel : BaseModel
{
    public AnswerBodyModel AnswerBodyModel;
    public string VideoKey;

    public int QuestionCount;
    public IList<QuestionInSet> QuestionsInSet;
    public int CurrentQuestion => AnswerBodyModel.QuestionId;

    public bool HideAddToKnowledge;
    public bool IsInWidget;

    public bool HasQuestion = false;

    public SetVideoModel(Set set, bool hideAddToKnowledge = false, bool isInWidget = false)
    {
        HideAddToKnowledge = hideAddToKnowledge;
        IsInWidget = isInWidget;

        VideoKey = set.VideoKey;

        if (set.QuestionsPublic().Any())
        {
            HasQuestion = true;

            var answerQuestionModel = new AnswerQuestionModel(set.QuestionsPublic().First());
            answerQuestionModel.DisableCommentLink = true;
            answerQuestionModel.DisableAddKnowledgeButton = HideAddToKnowledge;

            QuestionsInSet = set.QuestionsInSetPublic;
            AnswerBodyModel = new AnswerBodyModel(answerQuestionModel);
            AnswerBodyModel.DisableCommentLink = true;
        }

        QuestionCount = set.QuestionsPublic().Count;
    }
}