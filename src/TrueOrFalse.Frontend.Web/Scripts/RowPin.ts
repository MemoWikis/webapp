enum PinRowType {
    Question,
    Set
}

class PinRow {

    _changeInProgress: boolean;
    _pinRowType: PinRowType;

    constructor(pinRowType : PinRowType) {

        var self = this;
        this._pinRowType = pinRowType;

        $(".Pin").find(".iAdded, .iAddedNot").click(function (e) {

            var divPin = $($(this).parents(".Pin"));

            var id = -1;
            if(self._pinRowType == PinRowType.Question)
                id = parseInt(divPin.attr("data-question-id"));
            else if (self._pinRowType == PinRowType.Set)
                id = parseInt(divPin.attr("data-set-id"));

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) {

                self.Pin(id);
                divPin.find(".iAddedNot, .iAddSpinner").toggle();

                setTimeout(() => {
                    divPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow()) 
                        Utils.MenuPinsPluseOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) + 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(divPin) + 1, divPin);
                }, 400);

            } else {

                self.UnPin(id);
                divPin.find(".iAdded, .iAddSpinner").toggle();

                setTimeout(() => {
                    divPin.find(".iAddedNot, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow()) 
                        Utils.MenuPinsMinusOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) - 1).toString());
                        
                    self.SetSidebarValue(self.GetSidebarValue(divPin) - 1, divPin);
                }, 400);
            }
        });
    }

    SetSidebarValue(newValue: number, parent: JQuery) {
        Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
    }

    GetSidebarValue(parent: JQuery): number {
        return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find($(".totalPins")).html())[0]);
    }

    Pin(id: number) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id);
        } else if(this.IsSetRow()) {
            SetsApi.Pin(id);
        }
    }

    UnPin(id: number) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Unpin(id);
        } else if (this.IsSetRow()) {
            SetsApi.Unpin(id);
        }
    }

    IsQuestionRow(): boolean {
        return this._pinRowType == PinRowType.Question;
    }

    IsSetRow(): boolean {
        return this._pinRowType == PinRowType.Set;
    }
}