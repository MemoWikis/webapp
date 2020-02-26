declare var Tour: any;

var tourTemplate = "<div class='popover tour'>" +
            "<div class='arrow'> </div>" +
            "<h3 class='popover-title'> </h3>" +
            "<div class='popover-content'> </div>" +
            "<div class='popover-navigation'>" +
                "<button class='btn btn-sm btn-default' data-role='prev'>« Zurück </button>" +
                "<button class='btn btn-sm btn-default' data-role='next'> Weiter »</button>" +
                "<button class='btn btn-sm btn-default' data-role='end'>Beenden</button>" +
            "</div>" +
           "</div>";

// Instance the tour
var tourWelcome = new Tour({
    steps: [
        {
            element: "#mainMenuBtnKnowledge",
            backdrop: true,
            backdropPadding: 10,
            title: "Deine Wissenszentrale",
            content: "In deiner persönlichen Wissenszentrale hast du deinen Wissensstand immer im Blick, erhältst eine Auswertung zu deinem Lernverhalten, siehst, welche Termine und Lernsitzungen demnächst anstehen und was in deinem Netzwerk passiert."
        },
        {
            element: ".Pin:eq(1)",
            backdrop: true,
            backdropPadding: 10,
            placement: "auto top",
            title: "Fülle dein <i class='fa fa-heart' style='color:#b13a48;'></i> Wunschwissen",
            content: "Alles, was du gerne lernen oder wissen möchtest, kannst du zu deinem Wunschwissen hinzufügen. Klicke dazu auf das Herz-Symbol neben einer Frage, einem Lernset oder bei einem ganzen Thema."
        },
        {
            element: "#mainMenuQuestionsSetsCategories",
            backdrop: true,
            backdropPadding: 10,
            title: "Inhalte suchen oder neu erstellen",
            content: "Hier findest du alle Inhalte zum Lernen. Wenn du etwas nicht findest, kannst du die passenden Fragen einfach selbst erstellen."
        },
        {
            element: "#mainMenuBtnDates",
            backdrop: true,
            backdropPadding: 10,
            title: "Für eine Prüfung lernen",
            content: "Um für eine Prüfung zu lernen, erstellst du einfach einen Termin. memucho generiert für dich einen Lernplan, damit du genau weißt, wann und wieviel du noch lernen musst. Per Email erhältst du eine Erinnerung kurz vor jeder Lernsitzung."
        },
        {
            element: "#mainMenuBtnUsers",
            backdrop: true,
            backdropPadding: 10,
            title: "Dein Netzwerk",
            content: "Du kannst deinen Freunden bei memucho folgen. Dann siehst du, was sie gerade lernen, welche Fragen sie erstellt haben und kannst leichter mit ihnen zusammen ein Quiz spielen."
        },
        {
            element: "#mainMenuBtnGames",
            backdrop: true,
            backdropPadding: 10,
            title: "Quiz-Spiele in Echtzeit",
            content: "Apropo Quiz: Unter Spielen kannst du gegen deine Freunde in Echtzeit spielen oder gegen memucho antreten."
        },
        {
            element: "#boxLoginOrRegister", //#boxLoginOrRegister
            placement: "auto left",
            backdrop: true,
            backdropPadding: 10,
            onShow: function (tour) {
                if ($("#boxLoginOrRegister").width() == null) {
                    this.element = "";
                    this.title = "Los geht's!";
                    this.content = "Da du schon eingeloggt bist, kannst du gleich loslegen!";
                    this.orphan = true;
                }
            },
            title: "Einloggen oder Registrieren",
            content: "Registriere dich am besten gleich. Mit memucho sparst du Zeit beim Lernen und kannst dich entspannter auf Prüfungen vorbereiten."
        }
    ],
    storage: false, // tour starts at step 0 every time the page is reloaded
    template: tourTemplate,
    onNext: function(tour) {
        //console.log("onNext, left step-ID: " + tour.getCurrentStep());
    },
    onStart: function (tour) {
        //console.log("onStart");
    },
    onResume: function (tour) {
        //console.log("onResume");
    },
    onShow: function (tour) {
        //console.log("onShowGlobal");
    }
});
