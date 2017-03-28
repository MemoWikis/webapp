var scriptIndex;

class SetShare extends ShareDialog {

    private _setId: number;
    private _hasVideo: boolean;
    
    constructor(setId: number, hasVideo: boolean) {

        super();

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
        });
    }

    SetEmbedCode() {

        var host = Utils.GetHost();
        var url = host + "/views/widgets/w.js";
        var type = "set";

        if (this._hasVideo)
            type += "Video";

        var width = $("#widgetWidth").val() + $("#widgetWidthUnit").val();
        
        var maxWidth = "";
        if ($("#ckbEnableMaxWidth:checked").length == 1) {
            maxWidth = "maxWidth=\"" + $("#widgetMaxWidth").val() + "px\"";
        }

        var hideKnowledgeBtn = "";
        if ($("#ckbHideKnowledgeBtn:checked").length == 1) {
            hideKnowledgeBtn = "hideKnowledgeBtn=\"true\"";
        }

        var code =
            "<script src=\"" + url + "\" t=\"" + type + "\" id=\"" + this._setId +
            "\" width=\"" + width + "\" " + maxWidth + " " + hideKnowledgeBtn + "></script>";

        this.ShowEmbedCode(code, host);

    }

}