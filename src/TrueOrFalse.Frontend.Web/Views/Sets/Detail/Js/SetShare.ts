class SetShare {

    constructor(setId : number) {
        $("[data-action=embed-set]").click((e) => {
            e.preventDefault();
            this.ShowModal(setId);
        });
    }

    ShowModal(setId : number) {
        $.post("/Set/ShareSetModal?setId=" + setId, (modal) => {
            $("#modalShareSet").hide();
            $("#modalContainer").append(modal);
            this.InitModal(setId);
            $("#modalShareSet").modal('show');
        });        
    }

    InitModal(setId: number) {

        var url = "https://memucho.de/views/widgets/w.js";
        var type = "set";

        var embedCode =
            "<script src=\"" + url + "\" t=\"" + type + "\" id=\"" + setId + "\" width=\"100%\"></script>";

        $("#inputSetEmbedCode").val(embedCode);
    }

}