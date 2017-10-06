class SetCopy {

    constructor() {
        var self = this;
        var setToCopyId : number = parseInt($("#modalCopySet").attr("data-set-id"));

        $("#btnConfirmSetCopy").on("click",
            e => {
                e.preventDefault();
                self.CopySet(setToCopyId);
                //$('#modalCopySet').modal('hide');
            });
    }

    CopySet(sourceSetId: number) {
        $("#btnConfirmSetCopy").html("<i class=\"fa fa-spinner fa-spin\"> &nbsp;</i> Lernset wird kopiert...");
        $("#btnConfirmSetCopy").attr("disabled", "disabled");
        $("#btnAbortSetCopy").hide();

        var self = this;
        $.ajax({
            type: 'POST',
            url: "/Set/Copy/",
            data: { sourceSetId: sourceSetId },
            cache: false,
            success: function (result) {
                $("#modalCopySetHint").hide();
                $("#modalCopySet .modal-body").append(
                    "<div class='alert alert-success'><strong>Hurra!</strong> Du hast nun eine eigene Kopie des Lernsets und <a href='" + result.CopiedSetEditUrl + "'>kannst sie bearbeiten</a>.</div>");
                $("#modalCopySet .modal-footer").append(
                    "<a href='" + result.CopiedSetEditUrl + "' class='btn btn-primary'><i class='fa fa-arrow-right'>&nbsp; </i> Dein Lernset bearbeiten</a>");
                $("#btnConfirmSetCopy").hide();

            },
            error: function (e) {
                $("#modalCopySet .modal-body").append(
                    "<div class='alert alert-danger'>Ein Fehler ist aufgetreten, das Lernset wurde vermutlich nicht kopiert. Bitte melde dich bei uns, wenn der Fehler wiederholt auftritt.</div>");
                console.log(e);
            }
        });

    }

}