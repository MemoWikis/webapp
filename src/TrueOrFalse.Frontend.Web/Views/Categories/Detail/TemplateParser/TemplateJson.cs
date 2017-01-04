using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

[Serializable]
[DebuggerDisplay("TemplateName: {TemplateName}")]
public class TemplateJson
{
    public string TemplateName;

    public int CategoryId;

    public string Title;

    public string Description;

    public int SetId;

    public string SetText;

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

    public string CssClasses;
}
