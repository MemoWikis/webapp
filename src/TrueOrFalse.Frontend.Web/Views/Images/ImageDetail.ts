
class ImageDetailModal {
    static Init() {
        $('.JS-InitImageDetailModal').each(function () {

            $(this).click(
                function (e) {

                    var url = "/Images/ImageDetailModal?imgId=" + $(this).attr('data-image-id');

                    if ($(this).is("[data-is-youtube-video]")) {
                        url = "/Images/ImageDetailYoutubeModal?youtubeKey=" + $(this).attr('data-is-youtube-video');
                    }

                    e.preventDefault();
                    $.ajax({
                        type: 'POST',
                        url: url,
                        success: function (result) {
                            $('#modalImageDetail').remove();
                            $('#ModalImageDetailScript').remove();
                            $('.modal-backdrop.in').remove();
                            $(result).appendTo($('#MasterMainColumn'));
                            Images.ReplaceDummyImages();
                            $('#modalImageDetail').modal('show');
                        },
                    });
                }
                );
            $(this).removeClass('JS-InitImageDetailModal');
        });        
    }
}