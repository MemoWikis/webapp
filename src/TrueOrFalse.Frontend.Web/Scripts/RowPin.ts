enum PinRowType {
    Question,
    Set,
    SetDetail
}

class Pin {

    _changeInProgress: boolean;
    _pinRowType: PinRowType;
    _elemPin: JQuery;

    constructor(pinRowType : PinRowType) {

        var self = this;
        this._pinRowType = pinRowType;

        $(".Pin").find(".iAdded, .iAddedNot").click(function (e) {

            self._elemPin = $($(this).parents(".Pin"));

            var id = -1;
            if (self.IsQuestionRow())
                id = parseInt(self._elemPin.attr("data-question-id"));
            else if (self.IsSetRow() || self.IsSetDetail())
                id = parseInt(self._elemPin.attr("data-set-id"));

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) {

                self.Pin(id);
                self._elemPin.find(".iAddedNot, .iAddSpinner").toggle();

                window.setTimeout(() => {
                    self._elemPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow()) 
                        Utils.MenuPinsPluseOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) + 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(self._elemPin) + 1, self._elemPin);
                }, 400);

            } else {

                self.UnPin(id);
                
            }
        });
    }

    SetSidebarValue(newValue: number, parent: JQuery) {
        if (this.IsSetDetail())
            Utils.SetElementValue2(parent.find("#totalPins"), newValue.toString() + "x");
        else 
            Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
    }

    GetSidebarValue(parent: JQuery): number {
        if (this.IsSetDetail())
            return parseInt(/[0-9]*/.exec(parent.find("#totalPins").html())[0]);
        else
            return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find(".totalPins").html())[0]);
    }

    Pin(id: number) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id);
        } else if(this.IsSetRow() || this.IsSetDetail()) {
            SetsApi.Pin(id);
        }
    }

    UnPin(id: number) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Unpin(id);
        } else if (this.IsSetRow() || this.IsSetDetail()) {
            
            $("#UnpinSetModal").modal('show');

            $("#JS-RemoveQuestions").click(() => {
                SetsApi.Unpin(id);
                this.UpdateViewAfterUnpin();
            });

            $("#JS-DontRemoveQuestions").click(() => {
                SetsApi.Unpin(id, "false");
                this.UpdateViewAfterUnpin();
            });
        }
    }

    UpdateViewAfterUnpin() {
        var self = this;

        this._elemPin.find(".iAdded, .iAddSpinner").toggle();

        window.setTimeout(() => {
            self._elemPin.find(".iAddedNot, .iAddSpinner").toggle();
            self._changeInProgress = false;

            if (self.IsQuestionRow())
                Utils.MenuPinsMinusOne();

            Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) - 1).toString());

            self.SetSidebarValue(self.GetSidebarValue(self._elemPin) - 1, self._elemPin);
        }, 400);
    }

    IsQuestionRow(): boolean {
        return this._pinRowType == PinRowType.Question;
    }

    IsSetRow(): boolean {
        return this._pinRowType == PinRowType.Set;
    }

    IsSetDetail(): boolean {
        return this._pinRowType == PinRowType.SetDetail;
    }
}