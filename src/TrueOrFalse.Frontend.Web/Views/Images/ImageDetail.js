var fnInitImageDetailModal = function () {
    $('.JS-InitImageDetailModal').each(function () {
        $(this).click(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: "/Images/ImageDetailModal?imgId=" + $(this).attr('data-image-id'),
                success: function (result) {
                    $('#modalImageDetail').remove();
                    $('#ModalImageDetailScript').remove();
                    $('.modal-backdrop.in').remove();
                    $(result).appendTo($('#MasterMainColumn'));
                    $('#modalImageDetail').modal('show');
                }
            });
        });
        $(this).removeClass('JS-InitImageDetailModal');
    });
};
//# sourceMappingURL=ImageDetail.js.map
