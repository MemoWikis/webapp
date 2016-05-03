declare var Tour: any;

var tourTemplate = "<div class='popover tour'>" +
            "<div class='arrow'> </div>" +
            "<h3 class='popover-title'> </h3>" +
            "<div class='popover-content'> </div>" +
            "<div class='popover-navigation'>" +
                "<button class='btn btn-sm btn-default' data-role='prev'>« Zuruck </button>" +
                "<span data-role='separator'>|</span>" +
                "<button class='btn btn-sm btn-default' data-role='next'> Weiter »</button>" +
                "<button class='btn btn-sm btn-default' data-role='adjourn'><i class='fa fa-pause-circle-o'></i> Unterbrechen</button>" +
                "<button class='btn btn-sm btn-default' data-role='end'>Beenden</button>" +
            "</div>" +
           "</div>";

// Instance the tour
var tourWelcome = new Tour({
    steps: [
        {
            //element: "#Wordmark",
            orphan: true,
            title: "Willkommen bei memucho",
            content: "Mit memucho kannst du leichter und schneller lernen und behälst deinen Wissensstand immer im Blick. Wir zeigen dir nun in fünf Schritten, was du mit memucho machen kannst."
        },
        {
            element: "#menuWishKnowledgeCount",
            title: "Fülle dein <i class='fa fa-heart' style='color:#b13a48;'></i> Wunschwissen",
            content: "Alles, was du gerne lernen oder wissen möchtest, kannst du zu deinem Wunschwissen hinzufügen. Klicke dazu auf das Herz-Symbol neben einer Frage oder einem Fragesatz."
        },
        {
            element: "#mainMenuBtnQuestions",
            title: "Inhalte suchen oder neu erstellen",
            content: "Wenn du Inhalte lernen möchtest, die du noch nicht bei memucho findest, kannst du die passenden Fragen und Fragesätze einfach selbst erstellen."
        },
        {
            element: "#mainMenuBtnDates",
            backdrop: true,
            title: "Für eine Prüfung lernen",
            content: "Um für eine Prüfung zu lernen, erstellst du einfach einen Termin. memucho generiert für dich einen Übungsplan, damit du genau weißt, wann und wieviel du noch üben musst. Per Email erhälst du eine Erinnerung kurz vor jeder Übungssitzung."
        },
        {
            element: "#mainMenuBtnUsers",
            backdrop: true,
            title: "Dein Netzwerk",
            content: "Du kannst deinen Freunden bei memucho folgen. Dann siehst du, was sie gerade lernen, welche Fragen sie erstellt haben und kannst leichter mit ihnen zusammen ein Quiz spielen."
        },
        {
            element: "#mainMenuBtnGames",
            backdrop: true,
            backdropPadding: 20,
            title: "Quiz-Spiele in Echtzeit",
            content: "Apropo Quiz: Unter Spiele kannst du gegen deine Freunde in Echtzeit spielen oder gegen memucho antreten."
        },
        {
            element: "#mainMenuBtnKnowledge",
            title: "Deine Wissenszentrale",
            content: "Die Wunschwissen-Seite ist deine persönliche Wissenszentrale: Hier hast du deinen Wissensstand immer im Blick, erhälst eine Auswertung zu deinem Lernverhalten, siehst welche Termine und Übungssitzungen demnächst anstehen und was in deinem Netzwerk passiert."
        },
        {
            element: "#SupportUs",
            placement: "auto bottom",
            title: "Gute Idee? Unterstütze uns!",
            content: "Wenn dir gefällt, was wir machen, vergiss nicht uns zu unterstützen!"
        }
    ],
    onNext: function(tour) {
        console.log("onNext, left step-ID: " + tour.getCurrentStep());
    },

//    template: tourTemplate
});

//tourWelcome.onNext:

//function (parameters) {
//    console.log("onNext");
//}

