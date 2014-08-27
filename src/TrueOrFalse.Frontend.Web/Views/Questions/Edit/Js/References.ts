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
            true
        );
    }

    public AddFreetextReference() {
        $('#ReferenceSearchInput').hide();
        $('#AddFreeTextReference').show();

    }
}

class OnSelectForReference implements IAutocompleteOnSelect {
    
    OnSelect(autocomplete : AutocompleteCategories) {
        var existingReferences = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)');
        var refIdxes = new Array;
        for (var i = 0; i < existingReferences.length; i++) {
            refIdxes.push(parseInt($(existingReferences[i]).attr('data-ref-idx')));
        }
        var nextRefIdx = 1;
        if (existingReferences.length != 0) {
            nextRefIdx = Math.max.apply(Math, refIdxes) + 1;
        }
        $(
            "<div id='Ref-" + nextRefIdx + "' " + "data-ref-idx='" + nextRefIdx + "'" + "data-ref-id='" + autocomplete._referenceId + "'" + "class='JS-ReferenceContainer well'>" +
            "<a id='delete-ref-" + nextRefIdx + "'" + " class='close' href ='#'>×</a>" +
            "</div>").insertBefore('#JS-ReferenceSearch');
        $("#delete-ref-" + nextRefIdx).click(function (e) {
            e.preventDefault();
            $("#delete-ref-" + nextRefIdx).closest('.JS-ReferenceContainer').remove();
        });

        autocomplete._elemInput.val("");
        $('#JS-ReferenceSearch').hide();
        $('#AddReferenceControls').show();

        if (autocomplete._catId != -1) {
            $.ajax({
                url: '/Fragen/Bearbeite/ReferencePartial?catId=' + autocomplete._catId,
                type: 'GET',
                success: function (data) {
                    $('#Ref-' + nextRefIdx)
                        .append(data)
                        .append(
                        "<div class='form-group' style='margin-bottom: 0;'>" +
                            "<label class='columnLabel control-label' for='ReferenceAddition-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" +
                                "<div class='columnControlsFull'>" +
                                "<input class='InputRefAddition form-control input-sm' name='ReferenceAddition-" + nextRefIdx + "' type='text' placeholder='Seitenangaben etc.'/>" +
                            "</div>" +
                        "</div>")
                        .append("<input class='JS-hddRefCat' type='hidden' value='" + autocomplete._catId + "' name='ref-cat-" + nextRefIdx + "'/>");
                    $(window).trigger('referenceAdded' + autocomplete._referenceId);
                    $('.show-tooltip').tooltip();
                }
            });
        } else {
            $('#Ref-' + nextRefIdx)
                .append(
                "<div class='form-group' style='margin-bottom: 0;'>" +
                    "<div class='columnControlsFull'>" +
                        "<textarea class='FreeTextReference form-control' name='FreeTextReference-" + nextRefIdx + "' type='text' placeholder='Freitextquelle'></textarea>" +
                    "</div>" +
                "</div>");
            $(window).trigger('referenceAdded' + autocomplete._referenceId);
            $('.show-tooltip').tooltip();
        }        
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