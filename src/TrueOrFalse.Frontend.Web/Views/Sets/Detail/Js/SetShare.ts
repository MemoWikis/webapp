class SetShare {

    private _setId: number;
    private _hasVideo: boolean;
    
    constructor(setId: number, hasVideo: boolean) {
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

    InitModal() {
        this.InitSettings();  
        this.SetEmbedCode();
    }

    SetEmbedCode(){
        var url = "https://memucho.de/views/widgets/w.js";
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
            "<script src=\"" + url + "\" t=\"" + type + "\" id=\"" + this._setId + "\" width=\"" + width + "\" " + maxWidth + " " + hideKnowledgeBtn +"></script>";        

        $("#inputSetEmbedCode").val(code );
    }

    InitSettings()
    {
        var linkShowSettings = $("#modalShareSet [data-action=showSettings]");
        var linkHideSettings = $("#modalShareSet [data-action=hideSettings]");
        var divShareSetSettings = $("#divShareSetSettings");

        $("#widgetWidth").off("change").on("change", () => { this.SetEmbedCode(); });
        $("#widgetWidthUnit").off("change").on("change", () => { this.SetEmbedCode(); });

        $("#widgetMaxWidth").off("change").on("change", () => { this.SetEmbedCode(); });
        $("#ckbEnableMaxWidth").off("change").on("change", () => { this.SetEmbedCode(); });
        $("#ckbHideKnowledgeBtn").off("change").on("change", () => { this.SetEmbedCode(); });
        
        linkShowSettings.off("click").on("click", () => {
            divShareSetSettings.show();
            linkHideSettings.parent().show();
            linkShowSettings.parent().hide();
        });

        linkHideSettings.off("click").on("click", () => {
            divShareSetSettings.hide();
            linkHideSettings.parent().hide();
            linkShowSettings.parent().show();
        });
    }

}