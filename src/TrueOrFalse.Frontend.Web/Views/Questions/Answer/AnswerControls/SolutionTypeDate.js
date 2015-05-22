var SolutionTypeDateEntry = (function () {
    function SolutionTypeDateEntry() {
        var _this = this;
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });
        $("#txtAnswer").keyup(function () {
            _this.SetDateUi();
        });

        var metaData = this.GetJsonMetaData();
        $("#spanEntryPrecision").text(SolutionMetadataDate.GetPrecisionLabel(metaData.Precision) + "genau");
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
        $("#txtAnswer").select();
    };

    SolutionTypeDateEntry.prototype.SetDateUi = function () {
        var dateR = DateParser.Run($("#txtAnswer").val());

        if (!dateR.IsValid) {
            $("#spanEntryFeedback").html("kein g&#252;ltiges Datum");
            $("#iDateError").show();
            $("#iDateCorrect").hide();
        } else {
            $("#spanEntryFeedback").html("<b>" + SolutionMetadataDate.GetPrecisionLabel(dateR.Precision) + "genau </b>" + "(" + dateR.ToLabel() + ")");
            $("#iDateError").hide();
            $("#iDateCorrect").show();
        }
    };

    SolutionTypeDateEntry.prototype.GetJsonMetaData = function () {
        var jsonVal = $("#hddSolutionMetaDataJson").val();
        if (jsonVal.length == 0) {
            window.alert("Fehler: ungültige Frage");
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
