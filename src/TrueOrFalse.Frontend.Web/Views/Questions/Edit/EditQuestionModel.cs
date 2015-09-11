using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Web;

public class EditQuestionModel : BaseModel
{
    public UIMessage Message;

    public QuestionVisibility Visibility { get; set; }

    //[Required]
    [DataType(DataType.MultilineText )]
    [DisplayName("Frage")]
    public string Question { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Frage erweitert")]
    public string QuestionExtended { get; set; }

    [Required]
    [DisplayName("Antworttyp")]
    public string SolutionType { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Ergänzungen")]
    public string Description { get; set; }

    [DisplayName("Content rights")]
    [Required]
    public bool ConfirmContentRights { get; set;  }

    public int Id = -1;

    public IList<Category> Categories = new List<Category>();
    public IList<Reference> References = new List<Reference>();

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
                            new SelectListItem {Text = "Freie Antwort (Text/Zahl/Datum)", Value = TrueOrFalse.SolutionType.Text.ToString()},
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
        SolutionType = question.SolutionType.ToString();
        Description = question.Description;
        Categories = question.Categories;
        References = question.References;
        ImageUrl_128 = QuestionImageSettings.Create(question.Id).GetUrl_500px().Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);
        Visibility = question.Visibility;
    }

    public void FillCategoriesFromPostData(NameValueCollection postData)
    {
        Categories = AutocompleteUtils.GetRelatedCategoriesFromPostData(postData);
    }

    public IList<Reference> FillReferencesFromPostData(HttpRequestBase request, Question question)
    {
        var referencesJson = request["hddReferencesJson"];
        if(String.IsNullOrEmpty(referencesJson))
            References = new List<Reference>();

        return References = ReferenceJson.LoadFromJson(referencesJson, question);
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
        Description = "";
        Categories = new List<Category>();
    }
}