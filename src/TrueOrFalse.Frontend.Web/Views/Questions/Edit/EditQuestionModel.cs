using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Frontend.Web.Models;
using System.Linq;
using Message = TrueOrFalse.Web.Message;

public class EditQuestionModel : BaseModel
{
    public Message Message;

    [DisplayName("Sichtbarkeit")]
    public QuestionVisibility Visibility { get; set; }

    [Required]
    [DataType(DataType.MultilineText )]
    [DisplayName("Frage")]
    public string Question { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Frage erweitert")]
    public string QuestionExtended { get; set; }

    [Required]
    [DisplayName("Fragetyp")]
    public string SolutionType { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    [DisplayName("Fragetyp")]
    public string Solution { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Erklärungen")]
    public string Description { get; set; }

    public int? Id;

    public IEnumerable<String> Categories = new List<string>();

    public string PageTitle;
    public string FormTitle;
    public bool ShowSaveAndNewButton;
    public string ImageUrl_128;
    public string SoundUrl;
    public bool IsEditing;

    public IEnumerable<SelectListItem> VisibilityData { get {
            return new List<SelectListItem> {
                            new SelectListItem {Text = "Alle", Value = QuestionVisibility.All.ToString()},
                            new SelectListItem {Text = "Nur Ich", Value = QuestionVisibility.Owner.ToString()},
                            new SelectListItem {Text = "Ich und meine Freunde", Value = QuestionVisibility.OwnerAndFriends.ToString()},
                        };
        }
    }

    public IEnumerable<SelectListItem> AnswerTypeData{ get {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Standard", Value = TrueOrFalse.SolutionType.Text.ToString()},
                            new SelectListItem {Text = "Multiple Choice", Value = TrueOrFalse.SolutionType.MultipleChoice.ToString()},
                            new SelectListItem {Text = "Sequenz (Liste)", Value = TrueOrFalse.SolutionType.Sequence.ToString()},
                        };
        }
    }

    [DisplayName("Charakter")]
    public IEnumerable<SelectListItem> CharacterData{ get {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Ernsthaft", Value = QuestionCharacter.Serious.ToString()},
                            new SelectListItem {Text = "Unterhaltsam", Value = QuestionCharacter.Entertaining.ToString()}
                        };
        }
    }


    public EditQuestionModel()
    {
        ImageUrl_128 = "";
        SoundUrl = "";
    }

    public EditQuestionModel(Question question)
    {
        Id = question.Id;
        Question = question.Text;
        QuestionExtended = question.TextExtended;
        Solution = question.Solution;
        SolutionType = question.SolutionType.ToString();
        Description = question.Description;
        Categories = (from cat in question.Categories select cat.Name).ToList();
        ImageUrl_128 = new QuestionImageSettings(question.Id).GetUrl_500px().Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);
    }

    public void FillCategoriesFromPostData(NameValueCollection postData)
    {
        Categories = (from key in postData.AllKeys where key.StartsWith("cat") select postData[key]).ToList();
    }

    public void SetToCreateModel()
    {
        IsEditing = false;
        PageTitle = FormTitle = "Frage erstellen";
        ShowSaveAndNewButton = true;
    }

    public void SetToUpdateModel()
    {
        PageTitle = "Frage bearbeiten ("+ Question.Truncate(30, "...") + ")";
        FormTitle = string.Format("Frage '{0}' bearbeiten", Question.TruncateAtWord(30));
        IsEditing = true;
        ShowSaveAndNewButton = false;
    }

    public void Reset()
    {
        Id = -1;
        Question = "";
        Solution = "";
        Description = "";
        Categories = new string[]{};
    }
}