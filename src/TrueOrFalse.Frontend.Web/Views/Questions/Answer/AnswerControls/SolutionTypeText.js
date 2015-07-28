var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SolutionTypeTextEntry = (function (_super) {
    __extends(SolutionTypeTextEntry, _super);
    function SolutionTypeTextEntry(answerEntry) {
        _super.call(this, answerEntry);
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeTextEntry.prototype.GetAnswerText = function () {
        return $("#txtAnswer").val();
    };

    SolutionTypeTextEntry.prototype.GetAnswerData = function () {
        return { answer: $("#txtAnswer").val() };
    };

    SolutionTypeTextEntry.prototype.OnNewAnswer = function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();
    };
    return SolutionTypeTextEntry;
})(AnswerEntryBase);
;
//# sourceMappingURL=SolutionTypeText.js.map
