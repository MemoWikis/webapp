class QuestionsApi
{
    constructor() {
        eventBus.$on()
    }
    public static Pin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Pin/", { questionId: questionId }, (pinned) => {
            if (onPinChanged != null)
                onPinChanged();
            if (pinned) {
                var data = {
                    questionId: questionId,
                    isInWishknowledge: true,
                }
                eventBus.$emit('reload-wishknowledge-state-per-question', data);
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();

            }
        });
    }

    public static Unpin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId }, (unpinned) => {
            if (onPinChanged != null)
                onPinChanged();    
            if (unpinned) {
                var data = {
                    questionId: questionId,
                    isInWishknowledge: false,
                }
                eventBus.$emit('reload-wishknowledge-state-per-question', data);
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();

            }
        });
    }
} 