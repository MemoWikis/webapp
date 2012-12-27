using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;


public class QuestionSetsModel : BaseModel
{
    public Message Message;

    public int TotalQuestionSets { get; set; }
    public int TotalMine { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

 
}
