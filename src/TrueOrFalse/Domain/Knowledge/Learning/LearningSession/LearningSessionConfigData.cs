
public class LearningSessionConfigData
{
    public virtual int CategoryId { get; set; }
    public virtual string Mode { get; set; } = "Learning";
    public virtual bool IsInLearningTab { get; set; } = false;
    public virtual QuestionFilterJson QuestionFilter { get; set; } = null;
}
