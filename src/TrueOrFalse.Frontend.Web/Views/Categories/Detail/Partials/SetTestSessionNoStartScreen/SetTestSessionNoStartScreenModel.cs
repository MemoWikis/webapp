using System;

public class SetTestSessionNoStartScreenModel : BaseContentModule
{
    public Set Set;
    public string Title;
    public string Text;

    public SetTestSessionNoStartScreenModel(SetTestSessionNoStartScreenJson setTestSessionNoStartScreenJson)
    {
        Set = Sl.SetRepo.GetById(setTestSessionNoStartScreenJson.SetId);
        if (Set == null)
            throw new Exception("Die angegebene Lernset-ID verweist nicht auf ein existierendes Lernset.");
        Title = setTestSessionNoStartScreenJson.Title ?? Set.Name;
        Text = setTestSessionNoStartScreenJson.Text ?? Set.Text;
    }

}
