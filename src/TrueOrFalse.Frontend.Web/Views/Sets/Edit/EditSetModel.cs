using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Seedworks.Lib;
using TrueOrFalse.Web;

public class EditSetModel : BaseModel
{
    public UIMessage Message;

    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }
    public string ImageGuid { get; set; }
    public string ImageLicenseOwner { get; set; }

    public int Id { get; set; }
    public bool IsEditing { get; set; }

    [Required]
    [DisplayName("Titel")]
    public string Title { get; set;  }

    public string VideoUrl { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Beschreibung")]
    public string Text { get; set; }

    public IList<Category> Categories = new List<Category>(); 

    public string Username;

    public string ImageUrl_206px;

    public string PageTitle;
    public string FormTitle;

    public ISet<QuestionInSet> QuestionsInSet = new HashSet<QuestionInSet>();

    public Set Set;

    public EditSetModel(){
        if(IsLoggedIn)
            Username = new SessionUser().User.Name;

        ImageUrl_206px = new SetImageSettings(-1).GetUrl_206px_square().Url;    
    }

    public EditSetModel(Set set)
    {
        Id = set.Id;
        Title = set.Name;
        Text = set.Text;
        VideoUrl = set.VideoUrl;
        ImageUrl_206px = new SetImageSettings(set.Id).GetUrl_206px_square().Url;
        Username = new SessionUser().User.Name;
        QuestionsInSet = set.QuestionsInSet;
        Categories = set.Categories;
        Set = set;
    }

    public Set ToQuestionSet(){
        return Fill(new Set());
    }

    public Set Fill(Set set){
        set.Name = Title;
        set.Text = Text;
        set.VideoUrl = VideoUrl;

        ImageUrl_206px = new SetImageSettings(set.Id).GetUrl_206px_square().Url;
        QuestionsInSet = set.QuestionsInSet;

        FillCategoriesFromPostData(HttpContext.Current.Request.Form);

        set.Categories = Categories;

        this.Set = set;

        return set;
    }

    public void SetToCreateModel()
    {
        IsEditing = false;
        PageTitle = FormTitle = "Lernset erstellen";
    }

    public void SetToUpdateModel()
    {
        PageTitle = "Lernset bearbeiten (" + Title.Truncate(30, "...") +")";
        FormTitle = string.Format("Lernset '{0}' bearbeiten", Title.TruncateAtWord(30)); ;
        IsEditing = true;
    }

    public bool IsOwner(int userId)
    {
        return _sessionUser.IsLoggedInUser(userId);
    }

    public void FillCategoriesFromPostData(NameValueCollection postData)
    {
        Categories = AutocompleteUtils.GetRelatedCategoriesFromPostData(postData);
    }

}