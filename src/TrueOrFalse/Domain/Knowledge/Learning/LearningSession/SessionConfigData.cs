
public class SessionConfigData
{
    public int CategoryId { get; set; }
    public string Mode { get; set; } = "Learning";
    public bool IsInLearningTab { get; set; } = false;
    public QuestionFilterJson QuestionFilter { get; set; } = null;
}
