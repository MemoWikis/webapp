class WishKnowledgeContent {

    constructor() {

        WishKnowledgeContent.alertFadeInWhenNoWhisKnowledge(".topic", "#noWishKnowledge");
        $(".spinner").hide();
        $("#btnShowAllWishKnowledgeContent").click(function (e) {
            e.preventDefault();
            $("#wishKnowledgeMore").show();
            $("#btnShowAllWishKnowledgeContent").hide();
            $("#btnShowLessWishKnowledgeContent").show();
        });

        $("#btnShowLessWishKnowledgeContent").click(function (e) {
            e.preventDefault();
            $("#wishKnowledgeMore").hide();
            $("#btnShowAllWishKnowledgeContent").show();
            $("#btnShowLessWishKnowledgeContent").hide();
        });

        $(".fa-heart").on("click",
            function (e) {
                e.preventDefault();
                if ($(this).attr("data-set") != null) {
                    SetsApi.Unpin($(this).attr("data-id"));
                    $(this).closest(".topic").remove();
                    WishKnowledgeContent.alertFadeInWhenNoWhisKnowledge(".topic", "#noWishKnowledge");
                    return;
                }

                CategoryApi.Unpin($(this).attr("data-id"));
                $(this).closest(".topic").remove();
                WishKnowledgeContent.alertFadeInWhenNoWhisKnowledge(".topic", "#noWishKnowledge");

            });

        $(".link-to-topic").on("click",
            function (e) {
                e.preventDefault();
                window.location.replace("/Schule/" + $(this).attr("data-category") + "?openTab=learningTab");
            });

        $(".link-to-learnset").on("click",
            function (e) {
                e.preventDefault();
                window.location.replace("/Fragesatz/Lernen/" + $(this).attr("data-setId"));
            });

        $("#dashboard").click((e) => {
            e.preventDefault();
            this.LoadDashboard();
        });

        if (window.location.href.endsWith("Themen"))
            this.LoadTopicTab();

        if (window.location.href.endsWith("Fragen"))
            this.LoadQuestionsTab();

        if (window.location.href.endsWith("Ueberblick"))
            this.LoadDashboard();

        $("#topics").click((e) => {
            e.preventDefault();
            this.LoadTopicTab();
            
        });

        $("#questions").click((e) => {
            e.preventDefault();
            this.LoadQuestionsTab();
        });

        $(window).on('popstate', (e) => {
            var state = e.originalEvent.state;
            if (state.endsWith("Themen"))
                this.LoadTopicTab(/*pushState*/false);

            else if (state.endsWith("Fragen"))
                this.LoadQuestionsTab(/*pushState*/false);

            else if (state.endsWith("Ueberblick") || state.endsWith("Wissenszentrale"))
                this.LoadDashboard(/*pushState*/false);

        });
    }

    private LoadDashboard(pushState: boolean = true) {
        this.LoadTab({   
            stringTabId: "#dashboard",
            tabUrl: "/Knowledge/GetKnowledgeContent?content=dashboard",
            titleAction: "Lernsitzung starten",
            fnPushStateAction: () => {
                if (pushState)
                    window.history.pushState('Ueberblick', 'Ueberblick', '/Wissenszentrale/Ueberblick');
            }
        });
    }

    private LoadTopicTab(pushState: boolean = true) {
        this.LoadTab({
            stringTabId: "#topics",
            tabUrl: "/Knowledge/GetKnowledgeContent?content=topics",
            titleAction: "Thema erstellen",
            urlAction: $("#hddUrlAddTopic").val(),
            fnPushStateAction: () => {
                if (pushState)
                    window.history.pushState('Themen', 'Themen', '/Wissenszentrale/Themen');
            }
        });
    }

    private LoadQuestionsTab(pushState: boolean = true) {
        this.LoadTab({
            stringTabId: "#questions", 
            tabUrl: "/Knowledge/GetKnowledgeContent?content=questions",
            titleAction: "Frage erstellen", 
            urlAction: $("#hddUrlAddQuestion").val(),
            fnPushStateAction: () => {
                if (pushState)
                    window.history.pushState('Fragen', 'Fragen', '/Wissenszentrale/Fragen');
            }
        });
    }

    private LoadTab(loadTabParams: ILoadTabParams) {

        this.SetTabActiveString(loadTabParams.stringTabId);
        $.ajax({
            url: loadTabParams.tabUrl,
            type: "POST",
            beforeSend() { $(".spinner").fadeIn() },
        }).done(function (data) {
            $(".content").html(data);
            $(".LinkIsDirectedToPartialView").text(loadTabParams.titleAction); 
            if(loadTabParams.urlAction)
                $(".LinkIsDirectedToPartialView").attr("href", loadTabParams.urlAction);
            $(".spinner").hide();
        });

        loadTabParams.fnPushStateAction();
    }

    public SetTabActiveString(idString) {
        $("div.Tab").removeClass("active");
        $($(idString).parent()).addClass("active");
    }

    private static alertFadeInWhenNoWhisKnowledge(element: string, show: string) {
        if ($(element).length < 1) {
            $(show).show();
        }

    }

}

interface ILoadTabParams {
    stringTabId: string;
    tabUrl: string;
    titleAction: string; 
    urlAction?: string;
    fnPushStateAction: () => void;
}
