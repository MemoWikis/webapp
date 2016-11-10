class SetsApi
{
    public static Pin(setId, onPinChanged: () => void = null) {
        $.post("/Api/Sets/Pin/", { setId: setId }, () => {
            if(onPinChanged != null)
                onPinChanged();
        });
        Utils.SendGaEvent("WuWi", "Pin", "Set");
    }

    public static Unpin(setId, onPinChanged: () => void = null) {
        $.post("/Api/Sets/Unpin/", { setId: setId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
        Utils.SendGaEvent("WuWi", "Unpin", "Set");
    }

    public static UnpinQuestionsInSet(setId, onPinChanged: () => void = null) {
        $.post("/Api/Sets/UnpinQuestionsInSet/", { setId: setId }, () => {
            if (onPinChanged != null)
                onPinChanged();            
        });
        Utils.SendGaEvent("WuWi", "Unpin", "AllQuestionsInSet");
    }
}