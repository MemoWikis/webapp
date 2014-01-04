/// <reference path="../js/answerquestion.ts" />
var SolutionTypeDateEntry = (function () {
    function SolutionTypeDateEntry() {
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });

        var metaData = this.GetJsonMetaData();
        $("#spanEntryPrecision").text(SolutionMetadataDate.GetPrecisionLabel(metaData.Precision));
    }
    SolutionTypeDateEntry.prototype.GetAnswerText = function () {
        return $("#txtAnswer").val();
    };

    SolutionTypeDateEntry.prototype.GetAnswerData = function () {
        return { answer: $("#txtAnswer").val() };
    };

    SolutionTypeDateEntry.prototype.OnNewAnswer = function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    };

    SolutionTypeDateEntry.prototype.GetJsonMetaData = function () {
        var jsonVal = $("#hddSolutionMetaDataJson").val();
        if (jsonVal.length == 0) {
            alert("Fehler: ungueltige Frage");
            Logger.Error("no solution metaData");
        }

        return jQuery.parseJSON(jsonVal);
    };
    return SolutionTypeDateEntry;
})();

$(function () {
    new SolutionTypeDateEntry();
});
//# sourceMappingURL=SolutionTypeDate.js.map
