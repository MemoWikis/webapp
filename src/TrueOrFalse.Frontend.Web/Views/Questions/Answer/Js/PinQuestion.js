var PinQuestion = (function () {
    function PinQuestion() {
        var self = this;

        $("#iAdd").click(function (e) {
            var _this = this;
            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("fa-heart-o")) {
                self.Pin();
                $("#iAdd, #iAddSpinner").toggle();

                setTimeout(function () {
                    $(_this).switchClass("fa-heart-o", "fa-heart");
                    $("#iAdd, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsPluseOne();
                }, 400);
            } else {
                self.UnPin();
                $("#iAdd, #iAddSpinner").toggle();

                setTimeout(function () {
                    $(_this).switchClass("fa-heart", "fa-heart-o");
                    $("#iAdd, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsMinusOne();
                }, 400);
            }
        });
    }
    PinQuestion.prototype.Pin = function () {
        QuestionsApi.Pin(window.questionId);
    };

    PinQuestion.prototype.UnPin = function () {
        QuestionsApi.Unpin(window.questionId);
    };
    return PinQuestion;
})();

$(function () {
    new PinQuestion();
});
//# sourceMappingURL=PinQuestion.js.map
