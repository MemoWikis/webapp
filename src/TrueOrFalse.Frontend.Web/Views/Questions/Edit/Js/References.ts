class ReferenceJson {
    CategoryId: number;
    ReferenceId: number;
    ReferenceIndex: number;
    ReferenceType: ReferenceType;
    AdditionalText: string;
    ReferenceText : string;
}

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
    SearchFieldPlaceholder = "Suche nach Titel / Zeitschrift / Zeitung";
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

enum ReferenceType {
    MediaCategoryReference = 1,
    FreeTextreference = 2,
    UrlReference = 3,
}

class ReferenceUi
{
    private _referenceType: ReferenceType;

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
                if(referenceType == "FreeText")
                    this.AddFreetextReference();
                if(referenceType == "Url")
                    this.AddUrlReference();
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
        $('#AddFreeTextReference, #AddUrlReference').hide();
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').show();
        $('#ReferenceSearchInput').attr('placeholder', reference.SearchFieldPlaceholder);
        new AutocompleteCategories(
            "#ReferenceSearchInput",
            true,
            reference.FilterType,
            "",
            true
        );
    }

    public AddFreetextReference() {
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').hide();
        $('#AddUrlReference').hide();
        $('#AddFreeTextReference').show();
    }

    public AddUrlReference() {
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').hide();
        $('#AddFreeTextReference').hide();
        $('#AddUrlReference').show();
    }

    public static ReferenceToJson() : string {

        var jsonReferences: ReferenceJson[] = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)').map(function (idx, elem): ReferenceJson {
            var elemJ = $(elem);
            var result = new ReferenceJson();

            result.CategoryId = parseInt(elemJ.attr("data-cat-id"));
            result.ReferenceId = parseInt(elemJ.attr("data-ref-id"));
            //result.ReferenceIndex
            result.ReferenceType = this._referenceType;
            result.AdditionalText = elemJ.find("[name^='AdditionalInfo']").val();
            result.ReferenceText = elemJ.find("[name^='ReferenceText']").val();

            return result;
        }).toArray();

        return JSON.stringify(jsonReferences);;
    }
}

class OnSelectForReference implements IAutocompleteOnSelect {
    
    OnSelect(autocomplete : AutocompleteCategories, referenceId : number, referenceType: string) {
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
            "<div id='Ref-" + nextRefIdx + "' " +
                    "class='JS-ReferenceContainer well'" +
                    "data-ref-idx='" + nextRefIdx + "'" +
                    "data-ref-id='" + autocomplete._referenceId + "'" + 
                    "data-cat-id='" + autocomplete._catId + "'>" + 
            "<a id='delete-ref-" + nextRefIdx + "' class='close show-tooltip' href ='#' data-toggle='tooltip' title = 'Quellenangabe löschen' data-placement = 'top'>×</a>" +
            "</div>").insertBefore('#JS-ReferenceSearch');
        $("#delete-ref-" + nextRefIdx).click(function (e) {
            e.preventDefault();
            $("#delete-ref-" + nextRefIdx).closest('.JS-ReferenceContainer').remove();
        });

        autocomplete._elemInput.val("");
        $('#JS-ReferenceSearch').hide();
        $('#AddReferenceControls').show();

        if (autocomplete._referenceType != "FreeTextReference" && autocomplete._referenceType != "UrlReference") {
        //if (autocomplete._catId != -1) {
            $.ajax({
                url: '/Fragen/Bearbeite/ReferencePartial?catId=' + autocomplete._catId,
                type: 'GET',
                success: function (data) {
                    $('#Ref-' + nextRefIdx)
                        .append(data)
                        .append(
                        "<div class='form-group' style='margin-bottom: 0;'>" +
                        "<label class='columnLabel control-label' for='AdditionalInfo-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" +
                        "<div class='columnControlsFull'>" +
                        "<input class='InputRefAddition form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Seitenangaben, Zugriffsdatum etc.'/>" +
                        "</div>" +
                        "</div>");

                    $(window).trigger('referenceAdded' + referenceId);
                    $('.show-tooltip').tooltip();
                }
            });
        } else { /* No Category */
            if (referenceType == "FreeTextReference") {
                $('#Ref-' + nextRefIdx)
                    .append(
                    "<div class='form-group' style='margin-bottom: 0;'>" +
                    "<label class='columnLabel control-label' for='ReferenceText-" + nextRefIdx + "'>Freitextquelle</label>" +
                    "<div class='columnControlsFull'>" +
                    "<textarea class='FreeTextReference form-control input-sm' name='ReferenceText-" + nextRefIdx + "' type='text' placeholder='Quellenangabe'></textarea>" +
                    "</div>" +
                    "</div>");
            }
            if (referenceType == "UrlReference") {
                $('#Ref-' + nextRefIdx)
                    .append(
                    "<div class='form-group' style='margin-bottom: 0;'>" +
                        "<label class='columnLabel control-label' for='ReferenceText-" + nextRefIdx + "'>Url</label>" +
                        "<div class='columnControlsFull'>" +
                            "<input class='UrlReference form-control input-sm' name='ReferenceText-" + nextRefIdx + "' type='text' placeholder='Bitte hier nur die Url eingeben'/>" +
                            "<a href='#' id='TestLink-" + nextRefIdx + "' style='display: none;' target='_blank'>Link testen (in neuem Tab öffnen)</a>" +
                        "</div>" +
                    "</div>"+
                    "<div class='form-group' style='margin-bottom: 0;'>" +
                    "<label class='columnLabel control-label' for='AdditionalInfo-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" +
                        "<div class='columnControlsFull'>" +
                            //"<input class='UrlReference form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Zugriffsdatum etc.'/>" +
                            "<textarea class='UrlReference form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Zugriffsdatum etc.'></textarea>" +
                        "</div>" +
                    "</div>");

                var inputReferenceText = $('[name=ReferenceText-' + nextRefIdx + ']');
                inputReferenceText.bind('input blur', function (e) {
                    if ($(this).val() == "") {
                        $('#TestLink-' + nextRefIdx).hide();
                    } else if (e.type == "blur") {
                        var urlValue = inputReferenceText.val();
                        if (inputReferenceText.val().substring(0, 7) != "http://" && inputReferenceText.val().substring(0, 8) != "https://") {
                            urlValue = "http://" + urlValue;
                        }
                        inputReferenceText.val(urlValue);
                        $('#TestLink-' + nextRefIdx).show().attr('href', urlValue);
                    }
                });
            }
            $('#ReferenceSearchInput').data('referenceType', '');
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
            .data('referenceType', 'FreeTextReference')
            .trigger('initCategoryFromTxt');
    });
    $('#AddUrlReference button').click(function (e) {
        e.preventDefault();
        $("#ReferenceSearchInput")
            .data('category-id', '-1')
            .data('referenceType', 'UrlReference')
            .trigger('initCategoryFromTxt');
    });
});