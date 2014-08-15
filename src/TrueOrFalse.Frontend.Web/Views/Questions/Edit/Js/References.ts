
enum ReferenceType {
    Url, 
    Book,
    Periodicals
}

class Reference {
    Type: ReferenceType
    FilterType: AutoCompleteFilterType;
    LabelText = "";
    SearchFieldPlaceholder = "";


    constructor(type: ReferenceType) {
        this.Type = type;
    }

    ToHtml(): string { throw new Error('This method is abstract'); }
    Init(): void { throw new Error('This method is abstract'); }
}

class ReferenceUrl extends Reference {
    Url: string;

    constructor(url: string) {
        super(ReferenceType.Url);
        this.Url = url;
    }
}

class ReferencePeriodical extends Reference {
    CategoryPeriodicalId: number;
    CategoryIssueId: number;

    Year: number;
    IssueNo: number;
    Title: number;

    constructor() {
        super(ReferenceType.Periodicals);
    }
}

class ReferenceBook extends Reference {

    FilterType = AutoCompleteFilterType.Book;
    LabelText = "Buch suchen";
    SearchFieldPlaceholder = "Suche nach Buchtitel oder ISBN";

    constructor() {
        super(ReferenceType.Book);
    }
}

class ReferenceUi
{
    constructor() {
        var references = new Array<Reference>();
        //references.push(new ReferenceBook());
        for (var reference in references) {
            this.AddReferenceSearch(reference);
        }

        $("#AddReference").click((e) => {
            e.preventDefault();
            $('#JS-ReferenceSearch').empty();
            var referenceType = $('#ReferenceType option:selected').attr('value');
            if(referenceType == "Book")
                this.AddReferenceSearch(new ReferenceBook());
        });
    }

    public AddReferenceSearch(reference: Reference) {
        debugger;
        $("#JS-ReferenceSearch")
            .append("<label class='control-label LabelInline'>" + reference.LabelText + "</label>" +
                        "<div class='JS-CatInputContainer ControlInline'>" +
                "<input id='txtReference' class='form-control' name ='txtReference' type ='text' value ='' placeholder='" + reference.SearchFieldPlaceholder + "'/>" +
                "</div>");
        this.Init(reference.FilterType);
    }

    public Init(filterType: AutoCompleteFilterType) {
        new AutocompleteCategories("#txtReference", true, filterType);
    }
}

$(function () {
    new ReferenceUi();
});