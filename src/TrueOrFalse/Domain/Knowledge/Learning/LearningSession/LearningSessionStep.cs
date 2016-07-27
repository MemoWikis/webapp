using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

[JsonObject(MemberSerialization.OptIn)]
public class LearningSessionStep : DomainEntity, IRegisterAsInstancePerLifetime
{
    [JsonProperty]
    public int Idx { get; set; }

    private Question _question;

    public Question Question
    {
        get
        {
            if (_question != null)
                return _question;

            _question = Sl.R<QuestionRepo>().GetById(QuestionId);
            QuestionId = _question.Id;
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

            _answer = Sl.R<AnswerRepo>().GetById(AnswerId);
            AnswerId = _answer.Id;
            return _answer;
        }
        set
        {
            _answer = value;
            AnswerId = _answer.Id;
        }
    }

    [JsonProperty]
    public int AnswerId;

    [JsonProperty]
    public StepAnswerState AnswerState { get; set; }

    [JsonProperty]
    public bool IsRepetition { get; set; }

    public static void Skip(int stepId)
    {
        var stepRepo = Sl.Resolve<LearningSessionStepRepo>();
        var stepToSkip = stepRepo.GetById(stepId);
        if (stepToSkip != null && stepToSkip.AnswerState != StepAnswerState.Answered)
        {
            stepToSkip.AnswerState = StepAnswerState.Skipped;
            stepRepo.Update(stepToSkip);
        }
    }
}