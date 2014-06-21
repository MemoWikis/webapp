var PinRowType;
(function (PinRowType) {
    PinRowType[PinRowType["Question"] = 0] = "Question";
    PinRowType[PinRowType["Set"] = 1] = "Set";
})(PinRowType || (PinRowType = {}));

var PinRow = (function () {
    function PinRow(pinRowType) {
        var self = this;
        this._pinRowType = pinRowType;

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
    PinRow.prototype.SetSidebarValue = function (newValue, parent) {
        Utils.SetElementValue2(parent.parents(".question-row").find(".totalPins"), newValue.toString() + "x");
    };

    PinRow.prototype.GetSidebarValue = function (parent) {
        return parseInt(/[0-9]*/.exec(parent.parents(".question-row").find($(".totalPins")).html())[0]);
    };

    PinRow.prototype.Pin = function (questionId) {
        QuestionsApi.Pin(questionId);
    };

    PinRow.prototype.UnPin = function (questionId) {
        QuestionsApi.Unpin(questionId);
    };
    return PinRow;
})();
//# sourceMappingURL=RowPin.js.map
