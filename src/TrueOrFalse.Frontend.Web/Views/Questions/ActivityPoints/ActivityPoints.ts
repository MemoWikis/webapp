class ActivityPoints {

    public static AddPoitsFromRightAnswer() {
        ActivityPoints.addPoints(15, "RightAnswer");
    }

    public static  AddPoitsFromWrongAnswer() {
        ActivityPoints.addPoints(1, "WrongAnswer");
    }

    public static AddPoitsFromShowSolutionAnswer() {
        ActivityPoints.addPoints(3, "ShowSolution");
    }

    public static AddPointsFromCountAsCorrect() {
        ActivityPoints.addPoints(12, "CountAsCorrect");
    }

    private static addPoints(amount: number, actionTypeString: string) {
        var url = "/Api/ActivityPoints/Add/?points=" + amount + "&activityTypeString=" + actionTypeString;
        $.get(url, (result) => this.updatePointsDisplay(result.totalPoints));
    }

    private static updatePointsDisplay(totalPoints: number) {
        $("#activityPointsDispaly #activityPoints").html(totalPoints.toString());
    }
}