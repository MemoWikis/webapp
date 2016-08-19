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
            title: "Dein Überblick",
            content: "Diese Seite ist deine persönliche Wissenszentrale: Hier hast du deinen Wissensstand immer im Blick, erhälst eine Auswertung zu deinem Lernverhalten, siehst welche Termine und Übungssitzungen demnächst anstehen und was in deinem Netzwerk passiert."
        },
        {
            element: ".Pin:eq(1)",
            backdrop: true,
            backdropPadding: 10,
            placement: "auto top",
            title: "Fülle dein <i class='fa fa-heart' style='color:#b13a48;'></i> Wunschwissen",
            content: "Alles, was du gerne lernen oder wissen möchtest, kannst du zu deinem Wunschwissen hinzufügen. Klicke dazu auf das Herz-Symbol neben einer Frage oder einem Fragesatz."
        },
        {
            element: "#mainMenuQuestionsSetsCategories",
            backdrop: true,
            backdropPadding: 10,
            title: "Inhalte suchen oder neu erstellen",
            content: "Hier findest du alle Inhalte zum Lernen. Wenn du etwas nicht findest, kannst du die passenden Fragen und Fragesätze einfach selbst erstellen. Fragesätze beinhalten mehrere Fragen zu einem Thema."
        },
        {
            element: "#mainMenuBtnDates",
            backdrop: true,
            backdropPadding: 10,
            title: "Für eine Prüfung lernen",
            content: "Um für eine Prüfung zu lernen, erstellst du einfach einen Termin. memucho generiert für dich einen Übungsplan, damit du genau weißt, wann und wieviel du noch üben musst. Per Email erhälst du eine Erinnerung kurz vor jeder Übungssitzung."
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
            title: "Anmelden oder Registrieren",
            content: "Registriere dich am besten gleich, damit du mit memucho schneller und mit mehr Spaß lernen kannst."
        }
    ],
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

//tourWelcome.onNext:

//function (parameters) {
//    console.log("onNext");
//}

