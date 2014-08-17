class Reference {
    FilterType = AutoCompleteFilterType.None;
    LabelText = "";
    SearchFieldPlaceholder = "";
}

class ReferenceBook extends Reference {

    FilterType = AutoCompleteFilterType.Book;
    LabelText = "Buch suchen";
    SearchFieldPlaceholder = "Suche nach Buchtitel oder ISBN";
}

class ReferenceArticle extends Reference {

    FilterType = AutoCompleteFilterType.Article;
    LabelText = "Artikel suchen";
    SearchFieldPlaceholder = "Suche nach Titel oder Zeitschrift/Zeitung";
}

class ReferenceVolumeChapter extends Reference {

    FilterType = AutoCompleteFilterType.VolumeChapter;
    LabelText = "Beitrag in Sammelband suchen";
    SearchFieldPlaceholder = "Suche nach Titel oder Autor";
}

class ReferenceWebsiteArticle extends Reference {

    FilterType = AutoCompleteFilterType.WebsiteArticle;
    LabelText = "Online-Artikel suchen";
    SearchFieldPlaceholder = "Suche nach Titel oder Autor";
}

class ReferenceUi
{
    private _nextRefIdx = 1;

    constructor() {
        var references = new Array<Reference>();
        //references.push(new ReferenceBook());
        for (var reference in references) {
            this.AddReferenceSearch(reference);
        }

        $("#AddReference").click((e) => {
            e.preventDefault();
            //$('#JS-ReferenceSearch').empty();

            var referenceType = $('#ReferenceType option:selected').attr('value');
            if(referenceType == "Book")
                this.AddReferenceSearch(new ReferenceBook());
            if (referenceType == "Article")
                this.AddReferenceSearch(new ReferenceArticle());
            if (referenceType == "VolumeChapter")
                this.AddReferenceSearch(new ReferenceVolumeChapter());
            if (referenceType == "WebsiteArticle")
                this.AddReferenceSearch(new ReferenceWebsiteArticle());
        });
    }


    public AddReferenceSearch(reference: Reference) {
        var refSelector = "txtReference-" + this._nextRefIdx;
        this._nextRefIdx++;
        //$("#JS-ReferenceSearch")
        $("#JS-References")
            .append
            ("<div class='JS-ReferenceContainer well'>" +
            "<a class='close' data-dismiss='well' href ='#'>×</a>" +
            "<label class='control-label LabelInline'>" + reference.LabelText + "</label>" +
            "<div class='JS-CatInputContainer ControlInline'>" +
            "<input id='" + refSelector + "' class='form-control' name ='txtReference' type ='text' value ='' placeholder='" + reference.SearchFieldPlaceholder + "'/>" +
            "</div></div>");
        new AutocompleteCategories("#" + refSelector, true, reference.FilterType, ""
            , function (catId: string, catIdx: string, catName: string) { alert('Add cat "' + catName + ""); }, true
            );
        //this.Init(reference.FilterType);    
    }

    public Init(filterType: AutoCompleteFilterType) {
       
    }
}

$(function () {
    new ReferenceUi();
});