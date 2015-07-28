var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SolutionTypeMultipleChoice = (function (_super) {
    __extends(SolutionTypeMultipleChoice, _super);
    function SolutionTypeMultipleChoice(answerEntry) {
        _super.call(this, answerEntry);
        var answerQuestion = new AnswerQuestion(this);
        $('input:radio[name=answer]').change(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeMultipleChoice.prototype.GetAnswerText = function () {
        var selected = $('input:radio[name=answer]:checked');
        return selected.length ? selected.val() : "";
    };

    SolutionTypeMultipleChoice.prototype.GetAnswerData = function () {
        return { answer: $('input:radio[name=answer]:checked').val() };
    };

    SolutionTypeMultipleChoice.prototype.OnNewAnswer = function () {
        $('input:radio[name=answer]:checked').prop('checked', false);
    };
    return SolutionTypeMultipleChoice;
})(AnswerEntryBase);
;
//# sourceMappingURL=SolutionTypeMultipleChoice.js.map
