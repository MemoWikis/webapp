﻿class QuestionsApi
{
    public static Pin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Pin/", { questionId: questionId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
        Utils.SendGaEvent("WuWi", "Pin", "Question");
    }

    public static Unpin(questionId, onPinChanged: () => void = null) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId }, () => {
            if (onPinChanged != null)
                onPinChanged();    
        });
        
        Utils.SendGaEvent("WuWi", "Unpin", "Question");
    }
} 