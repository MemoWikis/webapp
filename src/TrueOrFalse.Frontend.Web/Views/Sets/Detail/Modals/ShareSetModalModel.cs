public class ShareSetModalModel
{
    public int SetId;
    public Set Set;
    public int DefaultQuestionCount = Settings.TestSessionQuestionCount;

    public ShareSetModalModel(int setId)
    {
        SetId = setId;
        Set = Sl.SetRepo.GetById(setId);
    }
}