class CategoryApi {

    public static Pin(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Pin/", { categoryId: categoryId }, (pinned) => {
            if (onPinChanged != null)
                onPinChanged();
            if (pinned) {
                eventBus.$emit('reload-knowledge-state');
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
            }
        });
    }

    public static Unpin(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Unpin/", { categoryId: categoryId }, (unpinned) => {
            if (onPinChanged != null)
                onPinChanged();
            if (unpinned)
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
        });
    }

    public static UnpinQuestionsInCategory(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/UnpinQuestionsInCategory/", { categoryId: categoryId }, (unpinned) => {
            if (onPinChanged != null)
                onPinChanged();
            if (unpinned) {
                eventBus.$emit('reload-knowledge-state');
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
            }
        });
    }

    static NavigatoTo(categoryName) {
        $.get("/Api/Category/GetUrl?categoryName=" + categoryName,
            function (data) { window.location.href = data; }
        );
    }
    
} 