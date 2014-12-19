declare function fnInitPopover(jObject: JQuery): void;

var fnInitImageDetailModal = function (jObject: JQuery) {
    jObject.each(function() {
        $(this).click(
            function (e) {
                e.preventDefault();
                $.ajax({
                    type: 'POST',
                    url: "/Maintenance/ImageDetailModal?imgId=" + $(this).attr('data-image-id'),
                    success: function(result) { 
                        $('#modalImageDetail').remove();
                        $('#ModalImageDetailScript').remove();
                        $('.modal-backdrop.in').remove();
                        $(result).appendTo($('#MasterMainColumn'));
                        $('#modalImageDetail').modal('show');
                    },
                });
            }
        );
    });
}