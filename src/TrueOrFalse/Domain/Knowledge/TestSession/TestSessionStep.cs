using System;

[Serializable]
public class TestSessionStep
{
    public int QuestionId;

    private Question _question; //im Model / Methode setzen

    public Question Question
    {
        get
        {
            if (_question != null)
                return _question;

            throw new Exception("Property Question of TestSessionStep must be set first before accessing it. Use TestSession.FillUpStepProperties.");
        }
        set
        {
            _question = value;
            QuestionId = _question.Id;
        }
    }

    public Guid QuestionViewGuid { get; set; }
    public TestSessionStepAnswerState AnswerState = TestSessionStepAnswerState.Uncompleted;

    public string AnswerText;
}