var drawKnowledgeChart;

class KnowledgeWheel {

    static ReloadCategory() {
        KnowledgeWheel.Reload("/KnowledgeWheel/GetForCategory/?categoryId=" + $("#hhdCategoryId").val());
    }

    private static Reload(url: string) {
        $.get(url, (html) => {

            $("#knowledgeWheelContainer")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            drawKnowledgeChart("chartKnowledge");
        });        
    }
}

class KnowledgeBar {
    public static ReloadCategory() {
        KnowledgeBar.Reload("/Category/KnowledgeBar/?categoryId=" + $("#hhdCategoryId").val());
        KnowledgeBar.ReloadForTopicNavs();
    }

    private static Reload(url: string) {
        $.get(url, (html) => {

            $("#CategoryHeader .KnowledgeBarWrapper")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            $("#CategoryHeader .KnowledgeBarWrapper .show-tooltip").tooltip();
        });
    }

    private static ReloadForTopicNavs() {
        $('#TopicTabContent .KnowledgeBarWrapper').each(function () {
            var categoryId = $(this).find('.category-knowledge-bar').attr('data-category-id');

            if (categoryId == undefined)
                throw "no category id ";

            $.get("/Category/KnowledgeBar/?categoryId=" + categoryId,
                (html) => {

                    $(this)
                        .empty()
                        .animate({ opacity: 0.00 }, 0)
                        .append(html)
                        .animate({ opacity: 1.00 }, 400);

                    $(this).find('.show-tooltip').tooltip();
                });

        });
    }
}