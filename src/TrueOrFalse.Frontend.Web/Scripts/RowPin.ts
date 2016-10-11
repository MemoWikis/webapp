enum PinRowType {
    Question,
    Set,
    SetDetail
}

class Pin {

    _changeInProgress: boolean;
    _pinRowType: PinRowType;

    OnPinChanged: () => void;

    constructor(pinRowType : PinRowType, onPinChanged : () => void = null) {

        var self = this;
        this._pinRowType = pinRowType;
        this.OnPinChanged = onPinChanged;

        var allPins;
        if (self.IsQuestionRow())
            allPins = $(".Pin[data-question-id]").find(".iAdded, .iAddedNot"); 
        else if (self.IsSetDetail() || self.IsSetRow())
            allPins = $(".Pin[data-set-id]").find(".iAdded, .iAddedNot"); 

        allPins.click(function (e) {

            e.preventDefault();
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg(); return;
            }

            var elemPin = $($(this).parents(".Pin"));

            var id = -1;
            if (self.IsQuestionRow())
                id = parseInt(elemPin.attr("data-question-id"));
            else if (self.IsSetRow() || self.IsSetDetail())
                id = parseInt(elemPin.attr("data-set-id"));

            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) /* pin */ {

                self.Pin(id, onPinChanged);
                elemPin.find(".iAddedNot, .iAddSpinner").toggle();

                window.setTimeout(() => {
                    elemPin.find(".iAdded, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow())
                        Utils.MenuPinsPluseOne();
                    else 
                        Utils.SetMenuPins();

                    if (self.IsSetDetail())
                        Utils.PinQuestionsInSetDetail();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) + 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(elemPin) + 1, elemPin);
                }, 400);

            } else /* unpin */ {

                self.UnPin(id, onPinChanged);
                elemPin.find(".iAdded, .iAddSpinner").toggle();

                window.setTimeout(() => {
                    elemPin.find(".iAddedNot, .iAddSpinner").toggle();
                    self._changeInProgress = false;

                    if (self.IsQuestionRow()) 
                        Utils.MenuPinsMinusOne();
                    //Update MenuPins after unpinning set is only necessary after user confirms in modal that questions are to be unpinned as well

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) - 1).toString());
                        
                    self.SetSidebarValue(self.GetSidebarValue(elemPin) - 1, elemPin);
                }, 400);
            }
        });

        $("#UnpinSetModal").off("click").on("click", "#JS-RemoveQuestions", function() {
            $.when(
                SetsApi.UnpinQuestionsInSet($('#JS-RemoveQuestions').attr('data-set-id'), onPinChanged))
                .then(function () {
                    Utils.UnpinQuestionsInSetDetail();
                    Utils.SetMenuPins();
                });
        });
    }

    SetSidebarValue(newValue: number, parent: JQuery) {
        if (this.IsSetDetail()) {
            Utils.SetElementValue2(parent.find("#totalPins"), newValue.toString() + "x");
            parent.find("#totalPins").attr("data-original-title", "Ist bei " + newValue.toString() + " Personen im Wunschwissen");
        } else {
            Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
            parent.parents(".rowBase").find(".totalPinsTooltip").attr("data-original-title", "Ist bei " + newValue.toString() + " Personen im Wunschwissen");
        }
    }

    GetSidebarValue(parent: JQuery): number {
        if (this.IsSetDetail())
            return parseInt(/[0-9]*/.exec(parent.find("#totalPins").html())[0]);
        else
            return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find(".totalPins").html())[0]);
    }

    Pin(id: number, onPinChanged: () => void = null) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id);
        } else if (this.IsSetRow() || this.IsSetDetail()) {
            SetsApi.Pin(id, onPinChanged);
        }
    }

    UnPin(id: number, onPinChanged: () => void = null) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Unpin(id);
        } else if (this.IsSetRow() || this.IsSetDetail()) {

            SetsApi.Unpin(id);

            $("#JS-RemoveQuestions").attr("data-set-id", id);

            $("#UnpinSetModal").modal('show');
        }
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