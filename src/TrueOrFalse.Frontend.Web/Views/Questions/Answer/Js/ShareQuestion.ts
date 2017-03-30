class ShareQuestion extends ShareDialog {

    private _questionId: number;

    constructor(questionId: number) {

        super("#modalShareQuestion");

        this._questionId = questionId;

        $("[data-action=embed-question]").click((e) => {
            e.preventDefault();
            this.ShowModal();
        });
    }

    ShowModal() {
        $.post("/AnswerQuestion/ShareQuestionModal?questionId=" + this._questionId, (modal) => {
            $("#modalShareQuestion").hide();
            $("#modalContainer").append(modal);
            this.InitModal();
            $("#modalShareQuestion").modal('show');
        });
    }
}