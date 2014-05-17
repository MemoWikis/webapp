
enum ReferenceType {
    Url, 
    Book,
    Periodicals
}

class Reference {
    Type: ReferenceType

    constructor(type : ReferenceType) {
        
    }
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

class ReferenceBooks extends Reference {
    CategoryBookId: number;
    CategoryEditionId: number;

    Title: number;

    constructor() {
        super(ReferenceType.Book);
    }
}

class ReferenceUi
{
    constructor() {
        var references = new Array<Reference>();
        references.push(new ReferenceUrl("http://someUrl"));
        references.push(new ReferenceUrl("http://someOtherUrl"));        

        for (var reference in references) {
            this.AddReference(reference);
        }
    }

    public AddReference(reference : Reference) {
        $("#references")
            .append($("<div class='xxs-stack col-xs-4'>")
                .append($("<select class='form-control'>")
                    .append($("<option>Url</option>"))
                    .append($("<optgroup label='Offline'>")
                        .append($("<option>Buch</option>"))
                        .append($("<option>Zeitung/Zeitschrift</option>"))
                    )
                )
            ).append($("<div class='xxs-stack col-xs-8'>")
                .append($("<input class='form-control' type='text' />")));                
    }
}

$(function () {
    new ReferenceUi();
});