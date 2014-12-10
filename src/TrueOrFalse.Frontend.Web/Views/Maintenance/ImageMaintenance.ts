var fnInitModal = function (jobject) {
    jobject.each(function() {
        $(this).click(
            function(e) {
                e.preventDefault();
                $.ajax({
                    type: 'POST',
                    url: "/Maintenance/ImageModal?imgId=" + $(this).attr('data-image-id'),
                    success: function(result) { 
                        $('#modalImageMaintenance').remove();
                        $('#ModalScript').remove();
                        $('.modal-backdrop.in').remove();
                        $(result).insertAfter($('table.ImageTable'));
                        $('#modalImageMaintenance').modal('show');
                    },
                });
            }
        );
    });
}

class ImageMaintenanceModal {

    constructor(imgId : number) {

        $('#SaveImageDataAndClose').click(function (e) {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: "/Maintenance/UpdateImage/",
                data: {
                    id: imgId,
                    authorManuallyAdded: $('#AuthorManuallyAdded').val(),
                    descriptionManuallyAdded: $('#DescriptionManuallyAdded').val(),
                    imageApproval: $("#ImageApproval").val()
                },
                success: function (result) {
                    var html = $(result);
                    $('tr#ImgId-' + imgId).replaceWith(html);
                    fnInitModal($('tr#ImgId-' + imgId + ' .ImageModal'));
                    $('#modalImageMaintenance').modal('hide');
                },
                //error: function (x, y) {
                //    alert('Das Bild konnte leider nicht gespeichert werden.');
                //}
            });
        });
    }
}