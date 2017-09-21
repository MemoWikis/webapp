class KnowledgeWheel {

    static ReloadSet() {
        KnowledgeWheel.Reload("/KnowledgeWheel/GetForSet/?setId=" + $("#hhdSetId").val());
    }

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
    static ReloadCategory() {
        KnowledgeBar.Reload("/Category/KnowledgeBar/?categoryId=" + $("#hhdCategoryId").val());
    }

    private static Reload(url: string) {
        debugger;
        $.get(url, (html) => {

            $(".KnowledgeBarWrapper")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            $(".KnowledgeBarWrapper .show-tooltip").tooltip();
        });
    }
}