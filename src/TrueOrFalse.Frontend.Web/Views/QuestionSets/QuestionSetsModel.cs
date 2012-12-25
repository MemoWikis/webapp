using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Frontend.Web.Models;


public class QuestionSetsModel : ModelBase
{
    public int TotalQuestionSets { get; set; }
    public int TotalMine { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

 
}
