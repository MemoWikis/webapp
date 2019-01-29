using System;

public class SetTestSessionNoStartScreenModel : BaseContentModule
{
    public Set Set;
    public string Title;
    public string Text;

    public SetTestSessionNoStartScreenModel(int setId, string title = null, string text = null)
    {
        Set = Sl.SetRepo.GetById(setId);
        if (Set == null)
            throw new Exception("Die angegebene Lernset-ID verweist nicht auf ein existierendes Lernset.");
        Title = title ?? Set.Name;
        Text = text ?? Set.Text;
    }

}
