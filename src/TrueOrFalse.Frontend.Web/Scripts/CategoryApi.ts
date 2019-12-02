class CategoryApi {

    public static Pin(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Pin/", { categoryId: categoryId }, () => {
            if (onPinChanged != null)
                onPinChanged();
            KnowledgeSummaryBar.updateKnowledgeSummaryBar();
        });
    }

    public static Unpin(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Unpin/", { categoryId: categoryId }, () => {
            if (onPinChanged != null)
                onPinChanged();
            KnowledgeSummaryBar.updateKnowledgeSummaryBar();
        });
    }

    public static UnpinQuestionsInCategory(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/UnpinQuestionsInCategory/", { categoryId: categoryId }, () => {
            if (onPinChanged != null)
                onPinChanged();
            KnowledgeSummaryBar.updateKnowledgeSummaryBar();
        });
    }

    static NavigatoTo(categoryName) {
        $.get("/Api/Category/GetUrl?categoryName=" + categoryName,
            function (data) { window.location.href = data; }
        );
    }
    
} 