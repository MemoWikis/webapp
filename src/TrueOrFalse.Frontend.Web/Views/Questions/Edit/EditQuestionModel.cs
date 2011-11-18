using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;
using Message = TrueOrFalse.Core.Web.Message;

public class EditQuestionModel : ModelBase
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

    [DisplayName("Schule/Uni")]
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
                            new SelectListItem {Text = "Exakt", Value = TrueOrFalse.Core.AnswerType.Exact.ToString()},
                            new SelectListItem {Text = "Annäherung", Value = TrueOrFalse.Core.AnswerType.Approximation.ToString()},
                            new SelectListItem {Text = "Multiple Choice", Value = TrueOrFalse.Core.AnswerType.MultipleChoice.ToString()},
                            new SelectListItem {Text = "Vokable", Value = TrueOrFalse.Core.AnswerType.Vocable.ToString()},
                            
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

    public EditQuestionModel()
    {
        ShowLeftMenu_Nav();
    }

    public EditQuestionModel(Question question) : this()
    {
        Question = question.Text;
        var answer = question.Answers[0];
        Answer = answer.Text;
        AnswerType = answer.Type.ToString();
        Description = question.Description;

    }

    public Question ConvertToQuestion()
    {
        var question = new Question();
        question.Answers.Add(new Answer());
        return UpdateQuestion(question);
    }

    public Question UpdateQuestion(Question question)
    {
        question.Text = Question;
        question.Description = Description;
        question.Answers[0].Text = Answer;
        return question;
    }

}

