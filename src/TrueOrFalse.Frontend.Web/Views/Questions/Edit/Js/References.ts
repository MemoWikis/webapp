
enum ReferenceType {
    Url, 
    Book,
    Periodicals
}

class Reference {
    Type: ReferenceType

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
    CategoryBookId: number;
    CategoryEditionId: number;

    Title: number;

    constructor() {
        super(ReferenceType.Book);
    }

    ToHtml() : string {
        return "" +
            "<label class='control-label LabelInline'>Buch suchen</label>" +
            "<div class='JS-CatInputContainer ControlInline'>" +
                "<input id='txtReferenceBook' class='form-control' name ='txtReferenceBook' type ='text' value ='' placeholder='Suche nach Titel oder ISSN'/>" + 
            "</div>";
    }

    Init(): void {
        new AutocompleteCategories("#txtReferenceBook", true, AutoCompleteFilterType.None);
    }

}

class ReferenceUi
{
    constructor() {
        var references = new Array<Reference>();
        for (var reference in references) {
            this.AddReference(reference);
        }

        $("#addReference").click((e) => {
            e.preventDefault();
            this.AddReference(new ReferenceBook());
        });
    }

    public AddReference(reference : Reference) {
        $("#JS-References").append(reference.ToHtml());
        reference.Init();
    }
}

$(function () {
    new ReferenceUi();
});