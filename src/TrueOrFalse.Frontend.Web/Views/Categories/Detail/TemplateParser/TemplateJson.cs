using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Serializable]
[DebuggerDisplay("TemplateName: {TemplateName}")]
public class TemplateJson
{
    public string TemplateName;

    public int ContainingCategoryId;//Doesn't have to be included in Json, is passed internally

    public string Load;
    public string Order;

    public string Title;

    public string Text;

    public int TitleRowCount;

    public string Description;

    public int DescriptionRowCount;

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

    public int SetRowCount;

    public int CategoryId;

    public string CssClasses;
}
