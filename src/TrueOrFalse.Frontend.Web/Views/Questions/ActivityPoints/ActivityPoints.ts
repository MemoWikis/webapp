class ActivityPoints {

    public static updatePointsDisplay() {
        $("#activityPointsDispaly #activityPoints").html(/*write Points from session in here*/);
    }

    public static addPoitsFromRightAnswer() {
        ActivityPoints.addPoints(15);
    }

    public static  addPoitsFromWrongAnswer() {
        ActivityPoints.addPoints(1);
    }

    public static addPoitsFromShowSolutionAnswer() {
        ActivityPoints.addPoints(3);
    }

    public static addPointsFromCountAsCorrect() {
        var amount; //evtl. abhängig von vorheriger Antwort
        ActivityPoints.addPoints(amount);
    }

    private static addPoints(amount: number) {
        //add points to session here
    }
}