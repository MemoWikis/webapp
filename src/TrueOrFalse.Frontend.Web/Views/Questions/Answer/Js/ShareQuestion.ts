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

        var self = this;

        $.post("/AnswerQuestion/ShareQuestionModal?questionId=" + this._questionId, (modal) => {
            $("#modalShareQuestion").hide();
            $("#modalContainer").append(modal);

            var interval = setInterval(() => {
                if ($("#ckbHideKnowledgeBtn").length === 1) {
                    self.InitModal();
                    clearInterval(interval);
                }
            }, 15);

            $("#modalShareQuestion").modal('show');
        });
    }

    SetEmbedCode() {

        var settings = this.GetSettings();
        settings.Id = this._questionId;
        settings.Type = "question";

        var code = this.GetEmbedCode(settings);

        this.ShowEmbedCode(code, settings.Host);
    }
}