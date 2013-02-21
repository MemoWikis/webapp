using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Seedworks.Lib;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class EditQuestionSetModel : BaseModel
{
    public Message Message;

    public int Id { get; set; }
    public bool IsEditing { get; set; }

    [Required]
    [DisplayName("Titel")]
    public string Title { get; set;  }

    [Required]
    [DataType(DataType.MultilineText)]
    [DisplayName("Beschreibung")]
    public string Text { get; set; }

    public string Username;

    public string PageTitle;
    public string FormTitle;

    public EditQuestionSetModel(){
        Username = new SessionUser().User.Name;
    }

    public EditQuestionSetModel(QuestionSet set){
        Id = set.Id;
        Title = set.Name;
        Text = set.Text;
    }

    public QuestionSet ToQuestionSet(){
        return Fill(new QuestionSet());
    }

    public QuestionSet Fill(QuestionSet questionSet){
        questionSet.Name = Title;
        questionSet.Text = Text;

        return questionSet;
    }

    public void SetToCreateModel()
    {
        IsEditing = false;
        PageTitle = FormTitle = "Fragesatz erstellen";
    }

    public void SetToUpdateModel()
    {
        PageTitle = "Fragesatz bearbeiten (" + Title.Truncate(30, "...") +")";
        FormTitle = string.Format("Fragesatz '{0}' bearbeiten", Title.TruncateAtWord(30)); ;
        IsEditing = true;
    }
}