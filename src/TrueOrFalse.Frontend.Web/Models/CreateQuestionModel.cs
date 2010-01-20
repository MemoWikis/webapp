using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class CreateQuestionModel
    {
        [Required]
        [DataType(DataType.MultilineText )]
        [DisplayName("Frage")]
        public string Question { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [DisplayName("Antwort")]
        public string Answer { get; set; }
    }
}
