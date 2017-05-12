using System;
using System.Collections.Generic;
using System.Linq;

public class SetVideoModel : BaseModel
{
    public AnswerBodyModel AnswerBodyModel;
    public string VideoKey;

    public int QuestionCount;
    public ISet<QuestionInSet> QuestionsInSet;
    public int CurrentQuestion => AnswerBodyModel.QuestionId;

    public bool HideAddToKnowledge;
    public bool IsInWidget;

    public bool HasQuestion = false;

    public SetVideoModel(Set set, bool hideAddToKnowledge = false, bool isInWidget = false)
    {
        HideAddToKnowledge = hideAddToKnowledge;
        IsInWidget = isInWidget;

        VideoKey = set.VideoKey;

        if (set.Questions().Any())
        {
            HasQuestion = true;

            var answerQuestionModel = new AnswerQuestionModel(set.Questions().First());
            answerQuestionModel.DisableCommentLink = true;
            answerQuestionModel.DisableAddKnowledgeButton = HideAddToKnowledge;

            QuestionsInSet = set.QuestionsInSet;
            AnswerBodyModel = new AnswerBodyModel(answerQuestionModel);
            AnswerBodyModel.DisableCommentLink = true;
        }

        QuestionCount = set.Questions().Count;
    }
}