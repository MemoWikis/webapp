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
            $.ajax({
                url: "/Knowledge/GetKnowledgeContent?content=dashboard",
                type: "POST",
                beforeSend() { $(".spinner").fadeIn() },
            }).done(function(data) {
                $(".content").html(data);
                $(".LinkIsDirectedToPartialView").text("Lernsitzung starten");
                $(".spinner").hide();
            });
        });

        $("#topics").click((e) => {
            e.preventDefault();
            this.SetTabActive(e);
            $.ajax({
                url: "/Knowledge/GetKnowledgeContent?content=topics",
                type: "POST",
                beforeSend() {$(".spinner").fadeIn()},
            }).done(function(data) {
                $(".content").html(data);
                $(".LinkIsDirectedToPartialView").text("Thema erstellen");
                $(".LinkIsDirectedToPartialView").attr("href", $("#hddUrlAddTopic").val());
                $(".spinner").hide();
            });
        });

        $("#questions").click((e) => {
            e.preventDefault();
            this.SetTabActive(e);
            $.ajax({
                url: "/Knowledge/GetKnowledgeContent?content=questions",
                type: "POST",
                beforeSend() { $(".spinner").fadeIn()},
            }).done(function(data) {
                $(".content").html(data);
                $(".LinkIsDirectedToPartialView").text("Frage erstellen"); //<%= Links.CreateQuestion() %>
                $(".LinkIsDirectedToPartialView").attr("href", $("#hddUrlAddQuestion").val());
                $(".spinner").hide();
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
