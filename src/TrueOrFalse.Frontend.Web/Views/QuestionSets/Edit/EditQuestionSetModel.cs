using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;


public class EditQuestionSetModel : BaseModel
{
    public Message Message;

    public bool IsEditing { get; set; }

    [Required]
    [DisplayName("Titel")]
    public string Title { get; set;  }

    [Required]
    [DataType(DataType.MultilineText)]
    [DisplayName("Beschreibung")]
    public string Text { get; set; }


    public QuestionSet ToQuestionSet()
    {
        var questionSet = new QuestionSet();
        questionSet.Name = Title;
        questionSet.Text = Text;
        return questionSet;
    }

}
