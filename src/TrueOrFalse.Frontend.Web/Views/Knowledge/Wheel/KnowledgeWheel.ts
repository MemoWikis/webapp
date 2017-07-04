class KnowledgeWheel {

    static ReloadSet() {
        KnowledgeWheel.Reload("/KnowledgeWheel/GetForSet/?setId=" + $("#hhdSetId").val());
    }

    static ReloadCategory() {
        KnowledgeWheel.Reload("/KnowledgeWheel/GetForCategory/?categoryId=" + $("#hhdCategoryId").val());
    }

    private static Reload(url: string) {
        $.get(url, (html) => {

            debugger;
            $("#knowledgeWheelContainer")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            drawKnowledgeChart("chartKnowledge");
        });        
    }
}