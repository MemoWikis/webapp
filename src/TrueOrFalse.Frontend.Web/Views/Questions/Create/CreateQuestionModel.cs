using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;
using Message = TrueOrFalse.Core.Web.Message;

public class CreateQuestionModel : ModelBase
{
    public Message Message;

    [DisplayName("Sichbar")]
    public QuestionVisibility Visibility { get; set; }

    [Required]
    [DataType(DataType.MultilineText )]
    [DisplayName("Frage")]
    public string Question { get; set; }

    [Required]
    [DisplayName("Antwort")]
    public string AnswerType { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    [DisplayName("Antwort")]
    public string Answer { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Erklärung")]
    public string Description { get; set; }

    [DisplayName("Verknüpfung Lehre")]
    public string EducationLink {get; set;}

    [DisplayName("Charakter")]
    public string Character { get; set; } 

    public IEnumerable<SelectListItem> EducationLinkData
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Fernuni Hagen, Elektrotechnik 1. Semester"},
                            new SelectListItem {Text = "- weitere Hinzufügen - "}
                        };
        }
    }


    public IEnumerable<SelectListItem> VisibilityData
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Alle", Value = QuestionVisibility.All.ToString()},
                            new SelectListItem {Text = "Nur Ich", Value = QuestionVisibility.Owner.ToString()},
                            new SelectListItem {Text = "Ich und meine Freunde", Value = QuestionVisibility.OwnerAndFriends.ToString()},
                        };
        }
    }


    public IEnumerable<SelectListItem> AnswerTypeData
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Exakt", Value = TrueOrFalse.Core.AnswerType.ExactText.ToString()},
                            new SelectListItem {Text = "Freitext", Value = TrueOrFalse.Core.AnswerType.FreeText.ToString()},
                            new SelectListItem {Text = "Multiple Choice", Value = TrueOrFalse.Core.AnswerType.MultipleChoice.ToString()},
                            new SelectListItem {Text = "Annäherung", Value = TrueOrFalse.Core.AnswerType.Approximation.ToString()}
                        };
        }
    }

    [DisplayName("Charakter")]
    public IEnumerable<SelectListItem> CharacterData
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Ernsthaft", Value = QuestionCharacter.Serious.ToString()},
                            new SelectListItem {Text = "Unterhaltsam", Value = QuestionCharacter.Entertaining.ToString()}
                        };
        }
    }

    public Question ConvertToQuestion()
    {
        var question = new Question();
        Question = Question;
        Answer = Answer;
        return question;
    }

    public CreateQuestionModel()
    {
        ShowLeftMenu_Nav();
    }

}

