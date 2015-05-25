var PinQuestion = (function () {
    function PinQuestion() {
    }
    PinQuestion.prototype.Init = function () {
        var self = this;

        $("#iAdded, #iAddedNot").click(function (e) {
            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).attr("id") == "iAddedNot") {
                self.Pin();
                $("#iAddedNot, #iAddSpinner").toggle();

                window.setTimeout(function () {
                    $("#iAdded, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsPluseOne();
                    self.SetSidebarValue(self.GetSidebarValue() + 1);
                }, 400);
            } else {
                self.UnPin();
                $("#iAdded, #iAddSpinner").toggle();

                window.setTimeout(function () {
                    $("#iAddedNot, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsMinusOne();
                    self.SetSidebarValue(self.GetSidebarValue() - 1);
                }, 400);
            }
        });
    };

    PinQuestion.prototype.SetSidebarValue = function (newValue) {
        Utils.SetElementValue("#sideWishKnowledgeCount", newValue.toString() + "x");
    };

    PinQuestion.prototype.GetSidebarValue = function () {
        return parseInt(/[0-9]*/.exec($("#sideWishKnowledgeCount").html())[0]);
    };

    PinQuestion.prototype.Pin = function () {
        QuestionsApi.Pin(AnswerQuestion.GetQuestionId());
    };

    PinQuestion.prototype.UnPin = function () {
        QuestionsApi.Unpin(AnswerQuestion.GetQuestionId());
    };
    return PinQuestion;
})();
//# sourceMappingURL=PinQuestion.js.map
