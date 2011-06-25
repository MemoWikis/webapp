using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class CreateQuestionModel : ModelBase
    {
        [DisplayName("Sichbar")]
        public QuestionVisibility Visibility { get; set; }

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

        [Required]
        [DataType(DataType.MultilineText )]
        [DisplayName("Frage")]
        public string Question { get; set; }

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

        [DataType(DataType.Text)]
        [DisplayName("Verknüpfung Lehre")]
        public string EducationLink {get; set;}

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

        [DataType(DataType.Text)]
        [DisplayName("Charakter")]
        public IEnumerable<SelectListItem> Character
        {
            get
            {
                return new List<SelectListItem>
                           {
                               new SelectListItem {Text = "Fakten Wissen"},
                               new SelectListItem {Text = "Didaktisch"},
                               new SelectListItem {Text = "Unterhaltung"},
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
