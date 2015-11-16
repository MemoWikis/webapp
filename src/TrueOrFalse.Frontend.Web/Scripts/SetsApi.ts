class SetsApi
{
    public static Pin(setId) {
        $.post("/Api/Sets/Pin/", { setId: setId });
    }

    public static Unpin(setId) {
        $.post("/Api/Sets/Unpin/", { setId: setId });
    }

    public static UnpinQuestionsInSet(setId) {
        $.post("/Api/Sets/UnpinQuestionsInSet/", { setId: setId });
    }
}