class ActivityPoints {
    public static addPointsFromRightAnswer() {
        ActivityPoints.addPoints(15, "RightAnswer");
    }

    public static  addPointsFromWrongAnswer() {
        ActivityPoints.addPoints(1, "WrongAnswer");
    }

    public static addPointsFromShowSolutionAnswer() {
        ActivityPoints.addPoints(3, "ShowedSolution");
    }

    public static addPointsFromCountAsCorrect() {
        ActivityPoints.addPoints(12, "CountAsCorrect");
    }

    private static addPoints(amount: number, actionTypeString: string) {
        var url = "/Api/ActivityPoints/Add";
        $.post(url, { activityTypeString: actionTypeString, points: amount} , result => {
            this.updatePointsDisplay(result);
            this.showLevelPopup(result.levelPopup);
        });
    }

    private static updatePointsDisplay(levelData) {
        $("#ActivityPointsDisplay #ActivityPoints").html(levelData.totalPoints.toString());
        $("#header-level-display text").html(levelData.userLevel);

        $("#NextLevelProgressPercentageDone").css("width", levelData.activityPointsPercentageOfNextLevel + "%");
        $("#NextLevelProgressSpanPercentageDone").text(levelData.activityPointsPercentageOfNextLevel + "%");
        $("#UserActivityPoints").text(levelData.totalPoints);
        $("#ProgressToNextLevel").text(levelData.activityPointsTillNextLevel);
        $("#NextActivityLevel").text(levelData.userLevel + 1);
    }

    private static showLevelPopup(levelPopup: string) {
        if (levelPopup != "" && $("#IsWidget").val() !== "true") {
            $("#levelPopupModal").remove();
            var levelPopupObject = $(levelPopup);
            levelPopupObject.find(".redirect-to-register").click(e => {
                $(".TextLinkWithIcon").click();
            });

            levelPopupObject.modal();
        }
    }
}