class CategoryApi {

    public static UnpinQuestionsInCategory(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/UnpinQuestionsInCategory/", { categoryId: categoryId }, (unpinned) => {
            if (onPinChanged != null)
                onPinChanged();
            if (unpinned) {
                eventBus.$emit('reload-knowledge-state');
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
                $('#UnpinCategoryModal').modal('hide');
            }
        });
    }

    static NavigatoTo(categoryName) {
        $.get("/Api/Category/GetUrl?categoryName=" + categoryName,
            function (data) { window.location.href = data; }
        );
    }
    
} 