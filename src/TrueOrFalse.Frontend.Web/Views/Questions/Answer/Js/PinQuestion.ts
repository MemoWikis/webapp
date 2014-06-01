class PinQuestion
{
    _changeInProgress : boolean;

    constructor() {

        var self = this;

        $("#iAdd").click(function (e) {

            e.preventDefault();
            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("fa-heart-o")) {

                self.Pin();
                $("#iAdd, #iAddSpinner").toggle();

                setTimeout(() => {
                    $(this).switchClass("fa-heart-o", "fa-heart");    
                    $("#iAdd, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsPluseOne();
                    self.SetSidebarValue(self.GetSidebarValue() + 1);
                }, 400);
                
            } else {
                
                self.UnPin();
                $("#iAdd, #iAddSpinner").toggle();

                setTimeout(() => {
                    $(this).switchClass("fa-heart", "fa-heart-o");
                    $("#iAdd, #iAddSpinner").toggle();
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