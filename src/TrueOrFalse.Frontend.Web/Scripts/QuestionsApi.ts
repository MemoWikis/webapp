class QuestionsApi
{
    public static Pin(questionId) {
        $.post("/Api/Questions/Pin/", { questionId: questionId });
    }

    public static Unpin(questionId) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId });
    }
} 