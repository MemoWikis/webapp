var PinRowType;
(function (PinRowType) {
    PinRowType[PinRowType["Question"] = 0] = "Question";
    PinRowType[PinRowType["Set"] = 1] = "Set";
    PinRowType[PinRowType["SetDetail"] = 2] = "SetDetail";
})(PinRowType || (PinRowType = {}));

var Pin = (function () {
    function Pin(pinRowType) {
        var self = this;
        this._pinRowType = pinRowType;

        $(".Pin").find(".iAdded, .iAddedNot").click(function (e) {
            var elemPin = $($(this).parents(".Pin"));

            var id = -1;
            if (self.IsQuestionRow())
                id = parseInt(elemPin.attr("data-question-id"));
            else if (self.IsSetRow() || self.IsSetDetail())
                id = parseInt(elemPin.attr("data-set-id"));

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) {
                self.Pin(id);
                elemPin.find(".iAddedNot, .iAddSpinner").toggle();

                setTimeout(function () {
                    elemPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow())
                        Utils.MenuPinsPluseOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) + 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(elemPin) + 1, elemPin);
                }, 400);
            } else {
                self.UnPin(id);
                elemPin.find(".iAdded, .iAddSpinner").toggle();

                setTimeout(function () {
                    elemPin.find(".iAddedNot, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow())
                        Utils.MenuPinsMinusOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) - 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(elemPin) - 1, elemPin);
                }, 400);
            }
        });
    }
    Pin.prototype.SetSidebarValue = function (newValue, parent) {
        if (this.IsSetDetail())
            Utils.SetElementValue2(parent.find("#totalPins"), newValue.toString() + "x");
        else
            Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
    };

    Pin.prototype.GetSidebarValue = function (parent) {
        if (this.IsSetDetail())
            return parseInt(/[0-9]*/.exec(parent.find("#totalPins").html())[0]);
        else
            return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find(".totalPins").html())[0]);
    };

    Pin.prototype.Pin = function (id) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id);
        } else if (this.IsSetRow() || this.IsSetDetail()) {
            SetsApi.Pin(id);
        }
    };

    Pin.prototype.UnPin = function (id) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Unpin(id);
        } else if (this.IsSetRow() || this.IsSetDetail()) {
            SetsApi.Unpin(id);
        }
    };

    Pin.prototype.IsQuestionRow = function () {
        return this._pinRowType == 0 /* Question */;
    };

    Pin.prototype.IsSetRow = function () {
        return this._pinRowType == 1 /* Set */;
    };

    Pin.prototype.IsSetDetail = function () {
        return this._pinRowType == 2 /* SetDetail */;
    };
    return Pin;
})();
//# sourceMappingURL=RowPin.js.map
