var fnEditCatValidation = function (categoryType) {
    var validationDefaultSettings = {
        rules: {
            Name: {
                required: true
            },
            WikipediaUrl: {
                url: true
            },
            Title: {},
            Year: {
                digits: true,
                minlength: 4,
                maxlength: 4
            },
            PublicationYear: {
                minlength: 4,
                maxlength: 4
            },
            PublicationDateMonth: {
                digits: true,
                maxlength: 2,
                range: [1, 12]
            },
            PublicationDateDay: {
                digits: true,
                maxlength: 2,
                range: [1, 31]
            }
        },
        messages: {
            Name: {},
            Year: {
                minlength: "Bitte gib das Jahr vierstellig an.",
                maxlength: "Bitte gib das Jahr vierstellig an."
            },
            PublicationYear: {
                minlength: "Bitte gib das Jahr vierstellig an.",
                maxlength: "Bitte gib das Jahr vierstellig an."
            },
            PublicationDateMonth: {
                range: "Bitte gib für den Monat eine Zahl von {0} bis {1} an."
            },
            PublicationDateDay: {
                range: "Bitte gib für den Tag eine Zahl von {0} bis {1} an."
            }
        }
    };

    var validationSettings = validationDefaultSettings;

    //Custom settings for partials
    if (categoryType == "Book") {
    }

    fnValidateForm("#EditCategoryForm", validationSettings);
};
//<span class="help-block">Ups, keine gültige Kategorie ausgewählt. Bitte suchen und aus der Liste auswählen oder <a>Kategorie in neuem Tab anlegen</a>.</span>
//# sourceMappingURL=Validation.js.map
