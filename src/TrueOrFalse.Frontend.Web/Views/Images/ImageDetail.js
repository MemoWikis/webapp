var ImageDetailModal = (function () {
    function ImageDetailModal() {
    }
    ImageDetailModal.Init = function () {
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
                        Images.ReplaceDummyImages();
                        $('#modalImageDetail').modal('show');
                    }
                });
            });
            $(this).removeClass('JS-InitImageDetailModal');
        });
    };
    return ImageDetailModal;
})();
//# sourceMappingURL=ImageDetail.js.map
