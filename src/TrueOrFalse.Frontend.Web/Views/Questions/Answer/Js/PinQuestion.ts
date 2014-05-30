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
                }, 400);
                
            } else {
                
                self.UnPin();
                $("#iAdd, #iAddSpinner").toggle();

                setTimeout(() => {
                    $(this).switchClass("fa-heart", "fa-heart-o");
                    $("#iAdd, #iAddSpinner").toggle();
                    self._changeInProgress = false;
                    Utils.MenuPinsMinusOne();
                }, 400);   
            }            
        });        
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