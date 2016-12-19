declare function fnInitPopover(jObject: JQuery): void;

var fnInitImageMaintenanceModal = (jObject: JQuery) => {
    jObject.each(function() {
        $(this).click(
            function(e) {
                e.preventDefault();
                $.ajax({
                    type: 'POST',
                    url: "/Maintenance/ImageMaintenanceModal?imgId=" + $(this).attr('data-image-id'),
                    success (result) { 
                        $('#modalImageMaintenance').remove();
                        $('#ImageMaintenanceModalScript').remove();
                        $('.modal-backdrop.in').remove();
                        $(result).insertAfter($('table.ImageTable'));
                        ImageMaintenanceModal.Show();
                    }
                });
            }
        );
    });
}

class ImageMaintenanceRow
{
    ImageId : number;

    constructor(imageId: number) {
        this.ImageId = imageId;
    }

    Hide() {
        console.log(this.ImageId);
        $("tr[id=ImgId-" + this.ImageId + "]").remove();
    }
}

class ImageMaintenanceModal {

    Row : ImageMaintenanceRow;
    
    constructor(imageId: number, manualImageEvaluationStringVal: string) {

        this.Row = new ImageMaintenanceRow(imageId);

        $('[data-toggle=popover]').popover({ html: true }).click(e => { e.preventDefault(); });

        $('#ManualImageEvaluation').val(manualImageEvaluationStringVal);

        $('#SaveImageDataAndClose').click(e => {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: "/Maintenance/UpdateImage/",
                data: {
                    id: imageId,
                    authorManuallyAdded: $('#AuthorManuallyAdded').val(),
                    descriptionManuallyAdded: $('#DescriptionManuallyAdded').val(),
                    manualImageEvaluation: $("#ManualImageEvaluation").val(),
                    remarks: $("#Remarks").val(),
                    selectedMainLicenseId: $("#SelectedMainLicenseId").val()
                },
                success(result) {
                    var html = $(result);
                    $('tr#ImgId-' + imageId).replaceWith(html);
                    fnInitImageMaintenanceModal($('tr#ImgId-' + imageId + ' .ImageMaintenanceModal'));
                    fnInitPopover($('tr#ImgId-' + imageId));
                    $('#modalImageMaintenance').modal('hide');

                    $('.ImageRowAlert').closest('tr').remove();
                    var message = html.find('#hddImageMaintenanceRowMessage-' + imageId).val() != null
                        ? html.find('#hddImageMaintenanceRowMessage-' + imageId).val()
                        : "";
                    if (message != "") {
                        var alert = $('<tr><td colspan = "5" style="padding-left: 0; padding-right: 0;">' +
                            '<div class="ImageRowAlert alert alert-info alert-dismissible" style="margin-bottom: 0;" role="alert">' +
                            '<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>' +
                            'Image ' + imageId + ': ' + message + '</div></td></tr>');
                        alert.insertAfter(html);
                        $('.ImageRowAlert').on('close.bs.alert', () => {
                            $('.ImageRowAlert').closest('tr').remove();
                        });
                    }
                },
                error() {
                    window.alert('Das Bild konnte leider nicht gespeichert werden.');
                }
            });
        });

        $('#ReloadImage').click(e => {
            e.preventDefault();

            var icon = $("#ReloadImage").find("i");
            icon.addClass("fa-spin");

            $("#Image").hide();
            $("#Image").attr("src", "");

            $.ajax({
                type: "POST",
                data: { imageMetaDataId : imageId},
                url: "/Maintenance/ImageReload/",
                success: result => {
                    icon.removeClass("fa-spin");
                    $("#Image").attr("src", result.Url);
                    $("#Image").show();
                },
                error: result => {
                    alert(result); console.log(result);
                }
            });
        });

        $('#DeleteImage').click(e => {
            e.preventDefault();

            var icon = $("#DeleteImage").find("i");
            icon.addClass("fa-spin");

            $.post("/Maintenance/ImageDelete/", { imageMetaDataId: imageId})
                .done(() => {
                    icon.removeClass("fa-spin");
                    ImageMaintenanceModal.Hide();
                    this.Row.Hide();
                })
                .fail(result => {
                    alert("Ein Fehler ist aufgetreten:" + result.responseText); console.log(result);
                });
        });
    }

    static Show() { $('#modalImageMaintenance').modal('show'); }

    static Hide() { $('#modalImageMaintenance').modal('hide'); }
}

$(() => {
    $("#ulLicenseStatus input").change(() => {
        (<any>window.document.forms[0]).submit();
    });
});