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

            var id = -1;
            if (self._pinRowType == 0 /* Question */)
                id = parseInt(divPin.attr("data-question-id"));
            else if (self._pinRowType == 1 /* Set */)
                id = parseInt(divPin.attr("data-set-id"));

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) {
                self.Pin(id);
                divPin.find(".iAddedNot, .iAddSpinner").toggle();

                setTimeout(function () {
                    divPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (!self.IsSetRow())
                        Utils.MenuPinsPluseOne();

                    self.SetSidebarValue(self.GetSidebarValue(divPin) + 1, divPin);
                }, 400);
            } else {
                self.UnPin(id);
                divPin.find(".iAdded, .iAddSpinner").toggle();

                setTimeout(function () {
                    divPin.find(".iAddedNot, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (!self.IsSetRow())
                        Utils.MenuPinsMinusOne();

                    self.SetSidebarValue(self.GetSidebarValue(divPin) - 1, divPin);
                }, 400);
            }
        });
    }
    PinRow.prototype.SetSidebarValue = function (newValue, parent) {
        Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
    };

    PinRow.prototype.GetSidebarValue = function (parent) {
        return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find($(".totalPins")).html())[0]);
    };

    PinRow.prototype.Pin = function (id) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id);
        } else if (this.IsSetRow()) {
            SetsApi.Pin(id);
        }
    };

    PinRow.prototype.UnPin = function (id) {
        if (this.IsQuestionRow()) {
            SetsApi.Unpin(id);
        } else if (this.IsSetRow()) {
            QuestionsApi.Unpin(id);
        }
    };

    PinRow.prototype.IsQuestionRow = function () {
        return this._pinRowType == 0 /* Question */;
    };

    PinRow.prototype.IsSetRow = function () {
        return this._pinRowType == 1 /* Set */;
    };
    return PinRow;
})();
//# sourceMappingURL=RowPin.js.map
