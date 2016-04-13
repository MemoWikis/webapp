class SetsApi
{
    public static Pin(setId, onPinChanged: () => void = null) {
        $.post("/Api/Sets/Pin/", { setId: setId }, () => {
            if(onPinChanged != null)
                onPinChanged();
        });
    }

    public static Unpin(setId, onPinChanged: () => void = null) {
        $.post("/Api/Sets/Unpin/", { setId: setId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
    }

    public static UnpinQuestionsInSet(setId, onPinChanged: () => void = null) {
        $.post("/Api/Sets/UnpinQuestionsInSet/", { setId: setId }, () => {
            if (onPinChanged != null)
                onPinChanged();            
        });
    }
}