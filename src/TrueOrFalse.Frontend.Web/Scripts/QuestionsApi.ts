class QuestionsApi
{
    public static Pin(questionId, onPinChanged: () => {} = null) {
        $.post("/Api/Questions/Pin/", { questionId: questionId });
        Utils.SendGaEvent("WuWi", "Pin", "Question");
    }

    public static Unpin(questionId, onPinChanged: () => {} = null) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId });
        Utils.SendGaEvent("WuWi", "Unpin", "Question");
    }
} 