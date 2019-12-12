class KnowledgeSummaryBar {
    public static updateKnowledgeSummaryBar() {
        var isInLearningTab = $('#LearningTab').length > 0;
        var categoryId = $('#hhdCategoryId').val();
        if (isInLearningTab)
            $.ajax({
                url: '/Category/RenderNewKnowledgeSummaryBar?categoryId=' + categoryId,
                type: 'GET',
                success: data => {
                    $(".category-knowledge-bar[data-category-id='" + categoryId + "']").replaceWith(data);
                    $('.show-tooltip').tooltip();
                },
            });
    }
}