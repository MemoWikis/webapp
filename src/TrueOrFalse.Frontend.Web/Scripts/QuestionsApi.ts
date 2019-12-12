class QuestionsApi
{
    public static Pin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Pin/", { questionId: questionId }, (pinned) => {
            if (onPinChanged != null)
                onPinChanged();
            if (pinned)
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
        });
    }

    public static Unpin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId }, (unpinned) => {
            if (onPinChanged != null)
                onPinChanged();    
            if (unpinned)
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
        });
    }
} 