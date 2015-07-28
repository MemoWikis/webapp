var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SolutionTypeNumeric = (function (_super) {
    __extends(SolutionTypeNumeric, _super);
    function SolutionTypeNumeric(answerEntry) {
        _super.call(this, answerEntry);
        this.IsGameMode = answerEntry.IsGameMode;

        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeNumeric.prototype.GetAnswerText = function () {
        return $("#txtAnswer").val();
    };

    SolutionTypeNumeric.prototype.GetAnswerData = function () {
        return { answer: $("#txtAnswer").val() };
    };

    SolutionTypeNumeric.prototype.OnNewAnswer = function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();
    };
    return SolutionTypeNumeric;
})(AnswerEntryBase);
//# sourceMappingURL=SolutionTypeNumeric.js.map
