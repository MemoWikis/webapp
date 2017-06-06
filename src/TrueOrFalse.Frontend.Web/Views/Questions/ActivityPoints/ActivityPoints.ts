class ActivityPoints {
    public static AddPointsFromRightAnswer() {
        ActivityPoints.addPoints(15, "RightAnswer");
    }

    public static  AddPointsFromWrongAnswer() {
        ActivityPoints.addPoints(1, "WrongAnswer");
    }

    public static AddPointsFromShowSolutionAnswer() {
        ActivityPoints.addPoints(3, "ShowedSolution");
    }

    public static AddPointsFromCountAsCorrect() {
        ActivityPoints.addPoints(12, "CountAsCorrect");
    }

    private static addPoints(amount: number, actionTypeString: string) {
        var url = "/Api/ActivityPoints/Add";
        $.post(url, { activityTypeString: actionTypeString, points: amount} , result => {
            this.updatePointsDisplay(result.totalPoints);
            this.showLevelPopup(result.levelPopup);
        });
    }

    private static updatePointsDisplay(totalPoints: number) {
        $("#activityPointsDispaly #activityPoints").html(totalPoints.toString());
    }

    private static showLevelPopup(levelPopup: string) {
        if (levelPopup != "") {
            $("#AnswerBody").append($(levelPopup));
            $(levelPopup).modal();
        }
    }
}