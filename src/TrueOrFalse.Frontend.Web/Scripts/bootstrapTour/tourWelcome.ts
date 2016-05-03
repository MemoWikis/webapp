declare var Tour: any;

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
            title: "Inhalte zu deinem Wunschwissen hinzufügen",
            content: "Alles, was du gerne lernen oder wissen möchtest, kannst du zu deinem Wunschwissen hinzufügen. Klicke dazu auf das Herz-Symbol neben einer Frage oder einem Fragesatz."
        }
    ]
});

