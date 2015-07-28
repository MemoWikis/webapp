var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SolutionTypeSequence = (function (_super) {
    __extends(SolutionTypeSequence, _super);
    function SolutionTypeSequence(answerEntry) {
        _super.call(this, answerEntry);
        var answerQuestion = new AnswerQuestion(this);
        $('.sequence-row').keydown(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeSequence.prototype.GetAnswerText = function () {
        return $.map($('.sequence-row'), function (x) {
            return $(x).val();
        }).join(", ");
    };

    SolutionTypeSequence.prototype.GetAnswerData = function () {
        return {
            answer: JSON.stringify($.map($('.sequence-row'), function (x) {
                return $(x).val();
            }))
        };
    };

    SolutionTypeSequence.prototype.OnNewAnswer = function () {
        $('.sequence-row').val("");
    };
    return SolutionTypeSequence;
})(AnswerEntryBase);
;
//# sourceMappingURL=SolutionTypeSequence.js.map
