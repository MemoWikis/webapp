using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Serializable]
[DebuggerDisplay("TemplateName: {TemplateName}")]
public class TemplateJson
{
    public string TemplateName;
    public string OriginalJson;

    [Obsolete]
    public int ContainingCategoryId;//Doesn't have to be included in Json, is passed internally
//
//    [Obsolete]
//    public string Load;
//
//    [Obsolete]
//    public string Order;
//
//    [Obsolete]
//    public string Title;
//
//    [Obsolete]
//    public string Text;
//
//    [Obsolete]
//    public int TitleRowCount;
//
//    [Obsolete]
//    public string Description;
//
//    [Obsolete]
//    public int DescriptionRowCount;
//
//    [Obsolete]
//    public int SetId;
//
//    [Obsolete]
//    public string SetText;
//
//    [Obsolete]
//    public string SetListIds;
//
//    [Obsolete]
//    public IList<Set> SetList
//    {
//        get
//        {
//            if (string.IsNullOrEmpty(SetListIds))
//            {
//                return new List<Set>();
//            }
//            var setIds = SetListIds
//                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
//                .Select(x => Convert.ToInt32(x));
//
//            var setRepo = Sl.R<SetRepo>();
//
//            return setIds
//                .Select(setId => setRepo.GetById(setId))
//                .Where(set => set != null)
//                .ToList();
//        }
//    }
//
//    [Obsolete]
//    public int SetRowCount;
//
//    [Obsolete]
//    public int CategoryId;
//
//    [Obsolete]
//    public string QuestionIds;
//
//    [Obsolete]
//    public int MaxQuestions;
//
//    [Obsolete]
//    public int AmountSpaces;
//
//    [Obsolete]
//    public bool AddBorderTop;
//
//    [Obsolete]
//    public string CssClasses;
//    
//    [Obsolete]
//    public string CardOrientation;

}
