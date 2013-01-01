using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Frontend.Web.Models;
using System.Linq;
using Message = TrueOrFalse.Web.Message;

public class EditQuestionModel : BaseModel
{
    public Message Message;

    [DisplayName("Sichtbar für")]
    public QuestionVisibility Visibility { get; set; }

    [Required]
    [DataType(DataType.MultilineText )]
    [DisplayName("Frage")]
    public string Question { get; set; }

    [Required]
    [DisplayName("Antwort")]
    public string SolutionType { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    [DisplayName("Antwort")]
    public string Solution { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Erklärung")]
    public string Description { get; set; }

    public int? Id;

    public IEnumerable<String> Categories = new List<string>();
    
    public string PageTitle;
    public bool ShowSaveAndNewButton;
    public string ImageUrl;
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
                            new SelectListItem {Text = "Sequenz", Value = TrueOrFalse.SolutionType.Sequence.ToString()},
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
        ImageUrl = "";
        SoundUrl = "";
    }

    public EditQuestionModel(Question question)
    {
        Id = question.Id;
        Question = question.Text;
        Solution = question.Solution;
        SolutionType = question.SolutionType.ToString();
        Description = question.Description;
        Categories = (from cat in question.Categories select cat.Name).ToList();
        ImageUrl = new GetQuestionImageUrl().Run(question).Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);
    }

    public void FillCategoriesFromPostData(NameValueCollection postData)
    {
        Categories = (from key in postData.AllKeys where key.StartsWith("cat") select postData[key]).ToList();
    }

    public void SetToCreateModel()
    {
        PageTitle = "Frage erstellen";
        ShowSaveAndNewButton = true;
    }

    public void SetToUpdateModel()
    {
        PageTitle = string.Format("Frage '{0}' bearbeiten", Question.TruncateAtWord(20));
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