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
        $("#AddReference").click((e) => {
            e.preventDefault();

            $('#JS-ReferenceSearch').show();
            $('#AddReferenceControls').hide();

            $("#ReferenceType").change(() => {

                var referenceType = $('#ReferenceType option:selected').attr('value');
                if(referenceType == "Book")
                    this.AddReferenceSearch(new ReferenceBook());
                if(referenceType == "Article")
                    this.AddReferenceSearch(new ReferenceArticle());
                if(referenceType == "VolumeChapter")
                    this.AddReferenceSearch(new ReferenceVolumeChapter());
                if(referenceType == "WebsiteArticle")
                    this.AddReferenceSearch(new ReferenceWebsiteArticle());
                if (referenceType == "FreeText")
                    this.AddFreetextReference();
            });

            $("#ReferenceType").trigger('change');

        });

        $('#JS-HideReferenceSearch').click((e) => {
            e.preventDefault();
            $('#JS-ReferenceSearch').hide();
            $('#AddReferenceControls').show();
        });
    }

    public AddReferenceSearch(reference: Reference) {
        $('#AddFreeTextReference').hide();
        $('#ReferenceSearchInput').show().attr('placeholder', reference.SearchFieldPlaceholder);
        new AutocompleteCategories(
            "#ReferenceSearchInput",
            true,
            reference.FilterType,
            "",
            function (catId: string, catIdx: string, catName: string) { alert('Add cat "' + catName + ""); },
            true
            );
    }

    public AddFreetextReference() {
        $('#ReferenceSearchInput').hide();
        $('#AddFreeTextReference').show();

    }
}

$(function () {
    new ReferenceUi();
    $('#AddFreeTextReference button').click(function(e) {
        e.preventDefault();
        $("#ReferenceSearchInput")
            .data('category-id', '-1')
            .trigger('initCategoryFromTxt');
    });
});