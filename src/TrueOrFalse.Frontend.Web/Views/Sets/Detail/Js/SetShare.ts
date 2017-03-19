class SetShare {

    constructor(setId : number, hasVideo : boolean) {
        $("[data-action=embed-set]").click((e) => {
            e.preventDefault();
            this.ShowModal(setId, hasVideo);
        });
    }

    ShowModal(setId: number, hasVideo: boolean) {
        $.post("/Set/ShareSetModal?setId=" + setId, (modal) => {
            $("#modalShareSet").hide();
            $("#modalContainer").append(modal);
            this.InitModal(setId, hasVideo);
            $("#modalShareSet").modal('show');
        });        
    }

    InitModal(setId: number, hasVideo: boolean) {

        var url = "https://memucho.de/views/widgets/w.js";
        var type = "set";

        if (hasVideo)
            type += "Video";

        var embedCode =
            "<script src=\"" + url + "\" t=\"" + type + "\" id=\"" + setId + "\" width=\"100%\"></script>";

        $("#inputSetEmbedCode").val(embedCode);
    }

}