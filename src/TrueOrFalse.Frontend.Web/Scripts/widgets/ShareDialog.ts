class ShareDialog {

    SetEmbedCode() { };

    ShowEmbedCode(code : string, host : string) {
        $("#inputSetEmbedCode").val(code);

        var codeElem = $(code);
        codeElem.attr("isPreview", "true");

        if (host == "http://memucho.local") {
            codeElem.attr("domainForDebug", host);
        }

        /* required for w.js: */ scriptIndex = -1;

        $("#divPreviewSetWidget").empty();
        $("#divPreviewSetWidget").append(codeElem);        
    }

    InitModal() {
        this.InitSettings();
        this.SetEmbedCode();
    }

    InitSettings() {
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

class Base {
    protected prop = null;

    constructor() {
        this.init();
        this.initLambda();
    }

    init() {
        console.log("Base init");
    }

    initLambda() {
        console.log("Base initLambda");
    }
}

class Derived extends Base {
    constructor() {
        super();
    }

    init() {
        console.log("Derived init");
    }

    initLambda() {
        console.log("Derived initLambda");
    }
}