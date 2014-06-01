class PinQuestion
{
    _changeInProgress : boolean;

    constructor() {

        var self = this;

        $("#iAdded, #iAddedNot").click(function (e) {

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).attr("id") == "iAddedNot") {

                self.Pin();
                $("#iAddedNot, #iAddSpinner").toggle();

                setTimeout(() => {
                    $("#iAdded, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsPluseOne();
                    self.SetSidebarValue(self.GetSidebarValue() + 1);
                }, 400);
                
            } else {
                
                self.UnPin();
                $("#iAdded, #iAddSpinner").toggle();

                setTimeout(() => {
                    $("#iAddedNot, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsMinusOne();
                    self.SetSidebarValue(self.GetSidebarValue() - 1);
                }, 400);   
            }            
        });        
    }

    SetSidebarValue(newValue : number) {
        Utils.SetElementValue("#sideWishKnowledgeCount", newValue.toString() + "x");
    }

    GetSidebarValue(): number {
        return parseInt(/[0-9]*/.exec($("#sideWishKnowledgeCount").html())[0]);
    }

    Pin() {
        QuestionsApi.Pin(window.questionId);
    }

    UnPin() {
        QuestionsApi.Unpin(window.questionId);
    }
          
}


$(function () {
    new PinQuestion();
});