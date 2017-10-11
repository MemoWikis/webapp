using System;

public class SetTestSessionAssessmentModel : BaseModel
{
    public Set Set;
    public string Title;
    public string Text;

    public SetTestSessionAssessmentModel(int setId, string title = null, string text = null)
    {
        Set = Sl.SetRepo.GetById(setId);
        if (Set == null)
            throw new Exception("Die angegebene Fragesatz-ID verweist nicht auf einen existierenden Fragesatz");
        Title = title ?? Set.Name;
        Text = text ?? Set.Text;
    }

}
