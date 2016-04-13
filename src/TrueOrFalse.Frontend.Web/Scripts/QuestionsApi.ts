class QuestionsApi
{
    public static Pin(questionId, onPinChanged: () => {} = null) {
        $.post("/Api/Questions/Pin/", { questionId: questionId });
    }

    public static Unpin(questionId, onPinChanged: () => {} = null) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId });
    }
} 