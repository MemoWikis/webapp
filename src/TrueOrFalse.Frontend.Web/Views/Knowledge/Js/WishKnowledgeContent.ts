class WishKnowledgeContent {

    constructor() {
        WishKnowledgeContent.alertFadeInWhenNoWhisKnowledge(".topic", "#noWishKnowledge");

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
                window.location.replace("/Kategorien/Schule/" + $(this).attr("data-category") + "?openTab=learningTab");
            });

        $(".link-to-learnset").on("click",
            function(e) {
                e.preventDefault();
                window.location.replace("/Fragesatz/Lernen/" + $(this).attr("data-setId"));
            });

        $("#dashboard").click((e) => {
            e.preventDefault();
            this.SetTabActive(e);
            $.post("/Knowledge/GetKnowledgeContent",
                {content: "dashboard"},
                function (data) {
                    $(".content").html(data);
                    $(".LinkIsDirectedToPartialView").text("Lernsitzung starten");
                });
        });

        $("#topics").click((e) => {
            e.preventDefault();
            this.SetTabActive(e);
            $.post("/Knowledge/GetKnowledgeContent",
                { content: "topics" },
                function (data) {
                    $(".content").html(data);
                    $(".LinkIsDirectedToPartialView").text("Thema erstellen");
                    $(".LinkIsDirectedToPartialView").attr("href",$("#hddUrlAddTopic").val());
                    
                });
        });

        $("#questions").click((e) => {
            e.preventDefault();
            this.SetTabActive(e);
            $.post("/Knowledge/GetKnowledgeContent",
                { content: "questions" },
                function (data) {
                    $(".content").html(data);
                    $(".LinkIsDirectedToPartialView").text("Frage erstellen");
                });
        });
    }

    public SetTabActive(e: JQueryEventObject) {
        $("div.Tab").removeClass("active");
        $($(e.target).parent()).addClass("active");
    }

    private static alertFadeInWhenNoWhisKnowledge(element: string, show: string) {
        if ($(element).length < 1) {
            $(show).show();
        } 
        
    }

}
