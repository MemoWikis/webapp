using System;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

[JsonObject(MemberSerialization.OptIn)]
public class LearningSessionStep
{
    [JsonProperty]
    public Guid Guid;

    [JsonProperty]
    public int Idx;

    private Question _question;

    public Question Question
    {
        get
        {
            if (_question != null)
                return _question;

            _question = Sl.R<QuestionRepo>().GetById(QuestionId);
            QuestionId = _question?.Id ?? -1;
            return _question;
        }
        set
        {
            _question = value;
            QuestionId = _question.Id;
        }
    }

    [JsonProperty]
    public int QuestionId;

    private Answer _answer;

    public Answer Answer
    {
        get
        {
            if (_answer != null)
                return _answer;

            return Sl.AnswerRepo.GetByLearningSessionStepGuid(Guid);
        }
        set => _answer = value;
    }

    [JsonProperty]
    public StepAnswerState AnswerState { get; set; }

    [JsonProperty]
    public bool IsRepetition { get; set; }
}