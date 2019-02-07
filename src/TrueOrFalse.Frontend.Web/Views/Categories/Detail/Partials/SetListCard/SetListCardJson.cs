using System;
using System.Collections.Generic;
using System.Linq;

public class SetListCardJson
{
    public string SetListIds;
    public IList<Set> SetList
    {
        get
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

    public string Title;
    public string Description;
    public int TitleRowCount;
    public int DescriptionRowCount;
    public int SetRowCount;
}
