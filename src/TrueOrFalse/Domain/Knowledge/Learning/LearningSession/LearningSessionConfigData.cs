
public class LearningSessionConfigData
{
    public virtual int CategoryId { get; set; }
    public virtual string Modus { get; set; } = "Learning";
    public bool IsInLearningTab { get; set; } = false;
    public virtual QuestionFilterJson QuestionFilter { get; set; } = null;
}
