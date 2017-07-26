class ShareDialog {

    _modalElemId: string;

    constructor(modalElemId: string) {
        this._modalElemId = modalElemId;
    }

    SetEmbedCode() { };

    ShowEmbedCode(code : string, host : string) {
        $("#inputSetEmbedCode").val(code);

        var codeElem = $(code);
        codeElem.attr("data-isPreview", "true");

        if (host == "http://memucho.local") {
            codeElem.attr("data-domainForDebug", host);
        }

        /* required for w.js: */ scriptIndex = -1;

        $("#divPreviewSetWidget").empty();
        $("#divPreviewSetWidget").append(codeElem);
    }

    InitModal() {
        this.InitSettings();
        this.SetEmbedCode();
        $('.show-tooltip').tooltip();
    }

    InitSettings() {
        var linkShowSettings = $(this._modalElemId + " [data-action=showSettings]");
        var linkHideSettings = $(this._modalElemId + " [data-action=hideSettings]");
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

    GetSettings(): WidgetSettings {

        var settings = new WidgetSettings();
        settings.Host = Utils.GetHost();
        settings.Url = settings.Host + "/views/widgets/w.js";

        settings.Width = $("#widgetWidth").val() + $("#widgetWidthUnit").val();

        settings.MaxWidth = "";
        if ($("#ckbEnableMaxWidth:checked").length == 1) {
            settings.MaxWidth = "data-maxWidth=\"" + $("#widgetMaxWidth").val() + "px\"";
        }

        settings.HideKnowledgeButton = "";
        if ($("#ckbHideKnowledgeBtn:checked").length == 1) {
            settings.HideKnowledgeButton = "data-hideKnowledgeBtn=\"true\"";
        }

        return settings;
    }

    GetEmbedCode(settings: WidgetSettings): string {
        return "<script src=\"" + settings.Url + "\" data-t=\"" + settings.Type + "\" data-id=\"" + settings.Id +
            "\" data-width=\"" + settings.Width + "\" " + settings.MaxWidth + " " + settings.HideKnowledgeButton + "></script>";
    }
}

class WidgetSettings {
    Host: string;
    Url: string;
    Width: string;
    MaxWidth: string;
    HideKnowledgeButton: string;

    Type: string;
    Id: number;
}