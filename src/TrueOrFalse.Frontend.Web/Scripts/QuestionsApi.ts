class QuestionsApi
{
    public static Pin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Pin/", { questionId: questionId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
    }

    public static Unpin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId }, () => {
            if (onPinChanged != null)
                onPinChanged();    
        });
    }
} 