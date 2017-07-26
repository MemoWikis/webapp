var scriptIndex;

class ShareSet extends ShareDialog {

    private _setId: number;
    private _hasVideo: boolean;
    
    constructor(setId: number, hasVideo: boolean) {

        super("#modalShareSet");

        this._setId = setId;
        this._hasVideo = hasVideo;

        $("[data-action=embed-set]").click((e) => {
            e.preventDefault();
            this.ShowModal();
        });     
    }

    ShowModal() {
        $.post("/Set/ShareSetModal?setId=" + this._setId, (modal) => {
            $("#modalShareSet").hide(); 
            $("#modalContainer").append(modal);
            this.InitModal();
            $("#modalShareSet").modal('show');

            var validationSettings = {
                rules: {
                    widgetQuestionCount: {
                        required: true,
                        digits: true
                    },
                    widgetWidth: {
                        required: false,
                        digits: true
                    },
                    widgetMaxWidth : {
                        required: false,
                        digits: true                        
                    }
                },
                errorPlacement: function (error, element) {
                    if (element.parent().attr("class") == "input-group") {
                        error.insertAfter($(element).parent());
                    }
                    else {
                        error.insertAfter(element);
                    }
                }
            };

            fnValidateForm("#modalShareSetForm", validationSettings, false);

        });
    }

    SetEmbedCode() {
        
        var type_ = "set";

        if (this._hasVideo)
            type_ += "Video";

        var settings = this.GetSettings();
        settings.Id = this._setId;
        settings.Type = type_;

        var code = this.GetEmbedCode(settings);

        this.ShowEmbedCode(code, settings.Host);
    }
}

