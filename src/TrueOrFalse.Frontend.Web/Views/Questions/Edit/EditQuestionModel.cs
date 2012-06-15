using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Frontend.Web.Models;
using System.Linq;
using Message = TrueOrFalse.Core.Web.Message;

public class EditQuestionModel : ModelBase
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

    [DisplayName("Charakter")]
    public string Character { get; set; }

    public IEnumerable<String> Categories = new List<string>();
    
    public string PageTitle;
    public bool ShowSaveAndNewButton;

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
                            new SelectListItem {Text = "Exakt", Value = QuestionSolutionType.Exact.ToString()},
                            new SelectListItem {Text = "Annäherung", Value = QuestionSolutionType.Approximation.ToString()},
                            new SelectListItem {Text = "Multiple Choice", Value = QuestionSolutionType.MultipleChoice.ToString()},
                            new SelectListItem {Text = "Vokable", Value = QuestionSolutionType.Vocable.ToString()},
                            new SelectListItem {Text = "Sequenz", Value = QuestionSolutionType.Sequence.ToString()},
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
    }

    public EditQuestionModel(Question question)
    {
        Question = question.Text;
        Solution = question.Solution;
        SolutionType = question.QuestionSolutionType.ToString();
        Description = question.Description;
        Categories = (from cat in question.Categories select cat.Name).ToList();
        
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

}

