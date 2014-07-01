fnAddRegExMethod("IsbnChar", /(^[0-9][-]{0,1}){0,}[xX0-9]$/, "Bitte verwende nur Ziffern und Bindestriche ('X' am Ende möglich).");
fnAddRegExMethod("IsbnLength", /(^(([a-z0-9][-]{0,1}){12})[a-z0-9]$)|(^(([a-z0-9][-]{0,1}){9})[a-z0-9]$)/, "Die ISBN muss genau 10 oder 13 Stellen haben (ohne Bindestriche).");
fnAddRegExMethod("IsbnAll", /(^(([0-9][-]{0,1}){12})[xX0-9]$)|(^(([0-9][-]{0,1}){9})[xX0-9]$)/, "Fies! Dieses Feld hat strenge Regeln: <br/> Verwende nur Ziffern und Bindestriche ('X' am Ende möglich).<br/> Die ISBN muss genau 10 oder 13 Stellen haben (ohne Bindestriche).");

var fnEditCatValidation = function (categoryType) {

    var getGroups = function () {
        var result = {};

        if (categoryType == "DailyIssue") {
            result = { DateGroup: "PublicationDateDay PublicationDateMonth Year" };
        }

        return result;
    };

    var validationDefaultSettings = {
        //debug: true,
        rules: {
            Name: {
                required: true,
            },
            Title: {
                required: true,
            },
            TitleVolume: {
                required: true,
            },
            Author: {
                required: true,
            },
            ISBN: {
                //IsbnChar: true,
                //IsbnLength: true,
                IsbnAll: true,
            },
            Year: {
                digits: true,
                minlength: 4,
                maxlength: 4,
            },
            PublicationYear: {
                minlength: 4,
                maxlength: 4,
            },
            PublicationDateMonth: {
                digits: true,
                maxlength: 2,
                range: [1, 12],
            },
            PublicationDateDay: {
                digits: true,
                maxlength: 2,
                range: [1, 31],
            },
            WikipediaUrl: {
                url: true,
            },
        },
        messages: {
            Name: {
            
            },
            Year: {
                minlength: "Bitte gib das Jahr vierstellig an.",
                maxlength:
                    "Bitte gib das Jahr vierstellig an.",
            },
            PublicationYear: {
                minlength: "Bitte gib das Jahr vierstellig an.",
                maxlength:
                "Bitte gib das Jahr vierstellig an.",
            },
            PublicationDateMonth: {
                range: "Bitte gib für den Monat eine Zahl von {0} bis {1} an.",
            },
            PublicationDateDay: {
                range: "Bitte gib für den Tag eine Zahl von {0} bis {1} an.",
            },
        },
        groups: getGroups(),
    }

    fnValidateForm("#EditCategoryForm", validationDefaultSettings);

    //Further custom settings for partials: 

    if (categoryType == "DailyArticle") {
        $('[name="Author"]').rules("add", { required: false, });
        $('[name="TxtDaily"]').rules("add", { required: true, });
        $('[name="TxtDailyIssue"]').rules("add", { required: true, });
    }

    if (categoryType == "DailyIssue") {
        $('[name="PublicationDateDay"]').rules("add", { required: true, });
        $('[name="PublicationDateMonth"]').rules("add", { required: true, });
        $('[name="Year"]').rules("add", { required: true, });
    }

    if (categoryType == "VolumeChapter") {
        $('[name="TitleVolume"]').rules("add", { required: true, });
        $('[name="Editor"]').rules("add", { required: true, });
    }
}


//<span class="help-block">Ups, keine gültige Kategorie ausgewählt. Bitte suchen und aus der Liste auswählen oder <a>Kategorie in neuem Tab anlegen</a>.</span> 