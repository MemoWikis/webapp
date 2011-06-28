using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using Message = TrueOrFalse.Core.Web.Message;

namespace TrueOrFalse.Frontend.Web.Models
{
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

        [DataType(DataType.Text)]
        [DisplayName("Hauptkategorie")]
        public string CategoryMain { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Unterkategorie")]
        public string CategorySub { get; set; }

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
                               new SelectListItem {Text = "Exakt", Value = Core.AnswerType.ExactText.ToString()},
                               new SelectListItem {Text = "Freitext", Value = Core.AnswerType.FreeText.ToString()},
                               new SelectListItem {Text = "Multiple Choice", Value = Core.AnswerType.MultipleChoice.ToString()},
                               new SelectListItem {Text = "Annäherung", Value = Core.AnswerType.Approximation.ToString()}
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

    
}
