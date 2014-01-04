/// <reference path="../js/answerquestion.ts" />

class SolutionTypeDateEntry implements ISolutionEntry {

    constructor() {
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(() => { answerQuestion.OnAnswerChange(); });

        var metaData = this.GetJsonMetaData();
        $("#spanEntryPrecision").text(SolutionMetadataDate.GetPrecisionLabel(metaData.Precision));
    }


    GetAnswerText(): string {
        return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
        return { answer: $("#txtAnswer").val() };
    }

    OnNewAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    }

    GetJsonMetaData(): SolutionMetadataDate {
        var jsonVal = $("#hddSolutionMetaDataJson").val();
        if (jsonVal.length == 0) {
            alert("Fehler: ungueltige Frage");
            Logger.Error("no solution metaData");
        }

        return <SolutionMetadataDate>jQuery.parseJSON(jsonVal);
    }

}


$(function () {
    new SolutionTypeDateEntry();
});