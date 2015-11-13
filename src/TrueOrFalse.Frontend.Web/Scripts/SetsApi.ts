class SetsApi
{
    public static Pin(setId) {
        $.post("/Api/Sets/Pin/", { setId: setId });
    }

    public static Unpin(setId, unpinQuestionsToo = "true") {
        $.post("/Api/Sets/Unpin/", { setId: setId, unpinQuestionsToo: unpinQuestionsToo });
    }
}