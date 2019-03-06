using System;
using System.Collections.Generic;
using System.Linq;

public class CardsJson
{
    public string TemplateName = "";
    public string Title = "";
    public string CardOrientation;
    public string SetListIds;

    public IList<Set> GetSetList()
    {
        if (string.IsNullOrEmpty(SetListIds))
        {
            return new List<Set>();
        }
        var setIds = SetListIds
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x));

        var setRepo = Sl.R<SetRepo>();

        return setIds
            .Select(setId => setRepo.GetById(setId))
            .Where(set => set != null)
            .ToList();
    }
}
