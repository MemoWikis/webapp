var PinQuestionRow = (function () {
    function PinQuestionRow() {
        var self = this;

        $(".Pin").find(".iAdded, .iAddedNot").click(function (e) {
            var divPin = $($(this).parents(".Pin"));
            var questionId = parseInt(divPin.attr("data-question-id"));

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) {
                self.Pin(questionId);
                divPin.find(".iAddedNot, .iAddSpinner").toggle();

                setTimeout(function () {
                    divPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsPluseOne();
                    self.SetSidebarValue(self.GetSidebarValue(divPin) + 1, divPin);
                }, 400);
            } else {
                self.UnPin(questionId);
                divPin.find(".iAdded, .iAddSpinner").toggle();

                setTimeout(function () {
                    divPin.find(".iAddedNot, .iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsMinusOne();
                    self.SetSidebarValue(self.GetSidebarValue(divPin) - 1, divPin);
                }, 400);
            }
        });
    }
    PinQuestionRow.prototype.SetSidebarValue = function (newValue, parent) {
        Utils.SetElementValue2(parent.parents(".question-row").find(".totalPins"), newValue.toString() + "x");
    };

    PinQuestionRow.prototype.GetSidebarValue = function (parent) {
        return parseInt(/[0-9]*/.exec(parent.parents(".question-row").find($(".totalPins")).html())[0]);
    };

    PinQuestionRow.prototype.Pin = function (questionId) {
        QuestionsApi.Pin(questionId);
    };

    PinQuestionRow.prototype.UnPin = function (questionId) {
        QuestionsApi.Unpin(questionId);
    };
    return PinQuestionRow;
})();

$(function () {
    new PinQuestionRow();
});
//# sourceMappingURL=QuestionRowPin.js.map
