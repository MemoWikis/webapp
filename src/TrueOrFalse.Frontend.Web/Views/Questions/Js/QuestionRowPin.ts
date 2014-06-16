class PinQuestionRow {
    _changeInProgress: boolean;

    constructor() {

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

                setTimeout(() => {
                    divPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsPluseOne();
                    self.SetSidebarValue(self.GetSidebarValue(divPin) + 1, divPin);
                }, 400);

            } else {

                self.UnPin(questionId);
                divPin.find(".iAdded, .iAddSpinner").toggle();

                setTimeout(() => {
                    divPin.find(".iAddedNot, .iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsMinusOne();
                    self.SetSidebarValue(self.GetSidebarValue(divPin) - 1, divPin);
                }, 400);
            }
        });
    }

    SetSidebarValue(newValue: number, parent: JQuery) {
        Utils.SetElementValue2(parent.parents(".question-row").find(".totalPins"), newValue.toString() + "x");
    }

    GetSidebarValue(parent: JQuery): number {
        return parseInt(/[0-9]*/.exec(parent.parents(".question-row").find($(".totalPins")).html())[0]);
    }

    Pin(questionId : number) {
        QuestionsApi.Pin(questionId);
    }

    UnPin(questionId : number) {
        QuestionsApi.Unpin(questionId);
    }
}


$(function () {
    new PinQuestionRow();
});