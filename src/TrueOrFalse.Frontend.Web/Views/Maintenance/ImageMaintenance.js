var fnInitModal = function (jObject) {
    jObject.each(function () {
        $(this).click(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: "/Maintenance/ImageModal?imgId=" + $(this).attr('data-image-id'),
                success: function (result) {
                    $('#modalImageMaintenance').remove();
                    $('#ModalScript').remove();
                    $('.modal-backdrop.in').remove();
                    $(result).insertAfter($('table.ImageTable'));
                    $('#modalImageMaintenance').modal('show');
                }
            });
        });
    });
};

var ImageMaintenanceModal = (function () {
    function ImageMaintenanceModal(imgId, manualImageEvaluationStringVal) {
        $('#ManualImageEvaluation').val(manualImageEvaluationStringVal);

        $('#SaveImageDataAndClose').click(function (e) {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: "/Maintenance/UpdateImage/",
                data: {
                    id: imgId,
                    authorManuallyAdded: $('#AuthorManuallyAdded').val(),
                    descriptionManuallyAdded: $('#DescriptionManuallyAdded').val(),
                    manualImageEvaluation: $("#ManualImageEvaluation").val(),
                    remarks: $("#Remarks").val(),
                    selectedMainLicenseId: $("#SelectedMainLicenseId").val()
                },
                success: function (result) {
                    var html = $(result);
                    $('tr#ImgId-' + imgId).replaceWith(html);
                    fnInitModal($('tr#ImgId-' + imgId + ' .ImageModal'));
                    fnInitPopover($('tr#ImgId-' + imgId));
                    $('#modalImageMaintenance').modal('hide');

                    $('.ImageRowAlert').closest('tr').remove();
                    var message = html.find('#hddImageMaintenanceRowMessage-' + imgId).val() != null ? html.find('#hddImageMaintenanceRowMessage-' + imgId).val() : "";
                    if (message != "") {
                        var alert = $('<tr><td colspan = "5" style="padding-left: 0; padding-right: 0;">' + '<div class="ImageRowAlert alert alert-info alert-dismissible" style="margin-bottom: 0;" role="alert">' + '<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>' + 'Image ' + imgId + ': ' + message + '</div></td></tr>');
                        alert.insertAfter(html);
                        $('.ImageRowAlert').on('close.bs.alert', function () {
                            $('.ImageRowAlert').closest('tr').remove();
                        });
                    }
                }
            });
        });
    }
    return ImageMaintenanceModal;
})();
//# sourceMappingURL=ImageMaintenance.js.map
